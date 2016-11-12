using Microsoft.Live;
using Microsoft.Phone.Shell;
using PhoneKit.Framework.Core.MVVM;
using PhoneKit.Framework.Core.Storage;
using PhoneKit.Framework.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhoneKit.Framework.Controls
{
    public abstract class BackupControlViewModelBase : ViewModelBase
    {
        public const string DEFAULT_BACKUP_NAME = "Backup";

        public const string BACKUP_ROOT = "Windows Phone/Backup";

        public string BasePath { get; private set; }

        private bool _isBusy;

        private bool _loginRequired = true;

        private string _backupName;

        private ObservableCollection<BackupItemViewModel> _backupItems = new ObservableCollection<BackupItemViewModel>();

        private DelegateCommand _loginCommand;

        private DelegateCommand<string> _backupCommand;

        private DelegateCommand<string> _restoreCommand;

        private DelegateCommand _refreshBackupsCommand;

        public BackupControlViewModelBase(string clientId, string appName)
        {
            InitializeCommands();
            OneDriveManager.Instance.InitializeLiveAuth(clientId);

            BasePath = string.Format(BACKUP_ROOT + "/{0}/", appName);
        }

        protected abstract IDictionary<string, IList<string>> GetBackupDirectoriesAndFiles();

        private void InitializeCommands()
        {
            _loginCommand = new DelegateCommand(async () =>
            {
                LoginRequired = !await Login();
            }, () =>
            {
                return !IsBusy && LoginRequired;
            });

            _backupCommand = new DelegateCommand<string>((string backupName) =>
            {
                Backup(backupName);
            }, (string backupName) =>
            {
                return !IsBusy && !LoginRequired;
            });

            _restoreCommand = new DelegateCommand<string>((string backupName) =>
            {
                Restore(backupName);
            }, (string backupName) =>
            {
                return !IsBusy && !LoginRequired;
            });

            _refreshBackupsCommand = new DelegateCommand(() =>
            {
                RefreshBackups();
            }, () =>
            {
                return !IsBusy && !LoginRequired;
            });
        }

        private async Task<bool> Login()
        {
            try
            {
                IsBusy = true;
                if (!OneDriveManager.Instance.IsLoggedIn)
                {
                    return await OneDriveManager.Instance.Login(GetScopes());
                }
            }
            catch (LiveAuthException)
            {
                return false;
            }
            finally
            {
                IsBusy = false;
            }
            return true;
        }

        protected abstract IEnumerable<string> GetScopes();

        //private static async Task<bool> LoginOrLogout()
        //{
        //    try
        //    {
        //        if (OneDriveManager.Instance.IsLoggedIn)
        //        {
        //            return OneDriveManager.Instance.Logout();
        //        }
        //        else
        //        {
        //            return await OneDriveManager.Instance.Login();
        //        }
        //    }
        //    catch (LiveAuthException)
        //    {
        //        return false;
        //    }
        //}

        protected virtual void UpdateCommands()
        {
            _loginCommand.RaiseCanExecuteChanged();
            _backupCommand.RaiseCanExecuteChanged();
            _restoreCommand.RaiseCanExecuteChanged();
            _refreshBackupsCommand.RaiseCanExecuteChanged();
        }

        private async void Backup(string backupName)
        {
            backupName = ValidateBackupName(backupName);

            IsBusy = true;
            var preservedIdleState = PhoneApplicationService.Current.UserIdleDetectionMode;
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            BeforeBackup(backupName);

            bool success = true;
            int numberOfElements = GetNumberOfElementsToUpload();

            // create base folder
            string baseFolderId = await OneDriveManager.Instance.CreateFolderPathAsync(OneDriveManager.ONEDRIVE_ROOT, BasePath + backupName);

            if (baseFolderId != null)
            {
                // upload files and folders
                var pathsAndFiles = GetBackupDirectoriesAndFiles();

                foreach (var path in pathsAndFiles.Keys)
                {
                    try
                    {
                        string folderId = await OneDriveManager.Instance.CreateFolderPathAsync(baseFolderId, path);

                        if (folderId != null)
                        {
                            foreach (var file in pathsAndFiles[path])
                            {
                                Stream dataStream = StorageHelper.GetFileStream(path + file);

                                if (dataStream != null)
                                {
                                    if (!await OneDriveManager.Instance.UploadAsync(folderId, file, dataStream))
                                    {
                                        success = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    success = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            success = false;
                            break;
                        }
                    }
                    catch(LiveConnectException lcex)
                    {
                        Debug.WriteLine("Live connect exception: " + lcex.Message);
                        success = false;
                        break;
                    }
                }
            }
            else
            {
                success = false;
            }

            AfterBackup(backupName, success);
            PhoneApplicationService.Current.UserIdleDetectionMode = preservedIdleState;
            IsBusy = false;
        }

        protected virtual void BeforeBackup(string backupName) { }

        protected virtual void AfterBackup(string backupName, bool success) { }

        private int GetNumberOfElementsToUpload()
        {
            int res = 0;
            var pathsAndFiles = GetBackupDirectoriesAndFiles();
            foreach (var files in pathsAndFiles.Values)
            {
                res += files.Count;
            }
            return res;
        }

        private async void Restore(string backupName)
        {
            backupName = ValidateBackupName(backupName);

            IsBusy = true;
            var preservedIdleState = PhoneApplicationService.Current.UserIdleDetectionMode;
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            BeforeRestore(backupName);
            bool success = true;

            // retrieve base folder
            string baseFolderId = await OneDriveManager.Instance.CreateFolderPathAsync(OneDriveManager.ONEDRIVE_ROOT, BasePath + backupName);

            if (baseFolderId != null)
            {
                if (!await OneDriveManager.Instance.DownloadRecursivly(baseFolderId, ""))
                {
                    success = false;
                }
            }
            else
            {
                success = false;
            }

            AfterRestore(backupName, success);
            PhoneApplicationService.Current.UserIdleDetectionMode = preservedIdleState;
            IsBusy = false;
        }

        protected virtual void BeforeRestore(string backupName) { }

        protected virtual void AfterRestore(string backupName, bool success) { }

        private async void RefreshBackups()
        {
            IsBusy = true;
            var preservedIdleState = PhoneApplicationService.Current.UserIdleDetectionMode;
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            BackupItems.Clear();

            // retrieve base folder
            string baseFolderId = await OneDriveManager.Instance.CreateFolderPathAsync(OneDriveManager.ONEDRIVE_ROOT, BasePath);

            dynamic folderList = await OneDriveManager.Instance.GetFolderListAsync(baseFolderId);

            var backupItemList = new List<BackupItemViewModel>();

            foreach (dynamic folder in folderList)
            {
                var backupItem = new BackupItemViewModel
                {
                    Name = folder.name,
                    BackupDate = DateTime.Parse(folder.updated_time)
                };
                backupItemList.Add(backupItem);
            }

            // sort
            backupItemList.Sort((a, b) => b.BackupDate.CompareTo(a.BackupDate));

            // pass to UI list
            foreach (var backupItem in backupItemList)
            {
                BackupItems.Add(backupItem);
            }

            PhoneApplicationService.Current.UserIdleDetectionMode = preservedIdleState;
            IsBusy = false;
        }

        private string ValidateBackupName(string backupName)
        {
            const char PLACEHOLDER = '_';

            // added default name, because Persian default was (why ever) NULL
            if (string.IsNullOrWhiteSpace(backupName)) {
                return DEFAULT_BACKUP_NAME;
            }

            // replace illegal chars with '_'
            foreach(var invalidChar in Path.GetInvalidPathChars())
            {
                backupName = backupName.Replace(invalidChar, PLACEHOLDER);
            }

            // replace additional chars
            backupName = backupName.Replace('/', PLACEHOLDER);
            backupName = backupName.Replace('\\', PLACEHOLDER);

            if (string.IsNullOrWhiteSpace(backupName))
                return DEFAULT_BACKUP_NAME;
            else
                return backupName;
        }

        public string BackupName
        {
            get { return _backupName; }
            set
            {
                if (value != _backupName)
                {
                    _backupName = value;
                    NotifyPropertyChanged("BackupName");
                }
            }
        }

        public ObservableCollection<BackupItemViewModel> BackupItems
        {
            get { return _backupItems; }
            private set { _backupItems = value; }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (value != _isBusy)
                {
                    _isBusy = value;
                    NotifyPropertyChanged("IsBusy");
                }
                UpdateCommands();
            }
        }

        public bool LoginRequired
        {
            get { return _loginRequired; }
            set
            {
                if (value != _loginRequired)
                {
                    _loginRequired = value;
                    NotifyPropertyChanged("LoginRequired");
                }
                UpdateCommands();
            }
        }

        public ICommand LoginCommand
        {
            get { return _loginCommand; }
        }

        public ICommand BackupCommand
        {
            get { return _backupCommand; }
        }

        public ICommand RestoreCommand
        {
            get { return _restoreCommand; }
        }

        public ICommand RefreshBackupsCommand
        {
            get { return _refreshBackupsCommand; }
        }
    }
}
