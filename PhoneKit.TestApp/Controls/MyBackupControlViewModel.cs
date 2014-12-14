using PhoneKit.Framework.Controls;
using PhoneKit.TestApp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhoneKit.TestApp.Controls
{
    public class MyBackupControlViewModel : BackupControlViewModelBase
    {
        public MyBackupControlViewModel()
            : base("000000004C119E36", AppResources.ApplicationTitle)
        {
            
        }

        protected override IDictionary<string, IList<string>> GetBackupDirectoriesAndFiles()
        {
            var pathsAndFiles = new Dictionary<string, IList<string>>();
            //pathsAndFiles.Add("/", new List<string> { "notes.data", "archive.data" });
            //pathsAndFiles.Add("Shared/ShellContent/attachements/", new List<string> { "65d46614-fb71-47f1-b9e8-8949627891af_wp_ss_20140909_0020.png", "944db2d7-7f54-44af-ac81-cb29c8971903_wp_ss_20140909_0020.png" });
            return pathsAndFiles;
        }

        protected override void BeforeBackup(string backupName)
        {
            base.BeforeBackup(backupName);

            //NoteListViewModel.Instance.Save();
            //ArchiveListViewModel.Instance.Save();
        }

        protected override void AfterBackup(string backupName, bool success)
        {
            base.AfterBackup(backupName, success);

            if (success)
            {
                MessageBox.Show(string.Format(AppResources.MessageBoxBackupSuccessText, backupName), AppResources.MessageBoxInfoTitle, MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show(string.Format(AppResources.MessageBoxBackupErrorText, backupName), AppResources.MessageBoxWarningTitle, MessageBoxButton.OK);
            }
        }

        protected override void AfterRestore(string backupName, bool success)
        {
            base.AfterRestore(backupName, success);

            if (success)
            {
                //NoteListViewModel.Instance.Load(true);
                //ArchiveListViewModel.Instance.Load(true);

                // remove tiles, because their reference link would be invalid
                //foreach (var note in NoteListViewModel.Instance.Notes)
                //{
                //    note.UnpinTile();
                //}

                MessageBox.Show(string.Format(AppResources.MessageBoxRestoreSuccessText, backupName), AppResources.MessageBoxInfoTitle, MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show(string.Format(AppResources.MessageBoxRestoreErrorText, backupName), AppResources.MessageBoxWarningTitle, MessageBoxButton.OK);
            }
        }
    }
}
