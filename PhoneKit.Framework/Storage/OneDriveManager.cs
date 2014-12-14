using Microsoft.Live;
using PhoneKit.Framework.Core.MVVM;
using PhoneKit.Framework.Core.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneKit.Framework.Storage
{
    public class OneDriveManager
    {
        public const string ONEDRIVE_ROOT = "me/skydrive";

        public static readonly string[] SCOPES_PHOTOS = new string[] { "wl.signin", "wl.skydrive_update", "wl.photos" };

        public static readonly string[] SCOPES_DEFAULT = new string[] { "wl.signin", "wl.skydrive_update" };

        private static OneDriveManager instance;

        public LiveAuthClient AuthClient { get; private set; }

        public LiveConnectClient LiveClient {get; private set; }

        public bool IsLoggedIn { get; set; }
        

        public void InitializeLiveAuth(string clientId)
        {
            try
            {
                AuthClient = new LiveAuthClient(clientId);
                //Login();
            }
            catch (LiveAuthException)
            {
                // TBD
            }
        }

        public async Task<bool> Login(IEnumerable<string> scopes)
        {
            LiveLoginResult loginResult;
            try
            {
                loginResult = await AuthClient.LoginAsync(scopes);
            }
            catch(Exception)
            {
                loginResult = null;
            }

            if (loginResult != null && loginResult.Status == LiveConnectSessionStatus.Connected)
            {
                IsLoggedIn = true;
                LiveClient = new LiveConnectClient(loginResult.Session);
            }
            else
            {
                IsLoggedIn = false;
            }

            return IsLoggedIn;
        }

        public bool Logout()
        {
            AuthClient.Logout();
            IsLoggedIn = false;
            return true;
        }

        public async Task<string> CreateFolderAsync(string location, string name)
        {
            var data = new Dictionary<string, object>();
            data.Add("name", name);
            try
            {
                LiveOperationResult operationResult = await LiveClient.PostAsync(location, data);
                dynamic result = operationResult.Result;
                return result.id;
            }
            catch
            {
                return null;
            }
            
        }

        public async Task<string> CreateFolderPathAsync(string location, string path)
        {
            if (path.EndsWith("/"))
                path = path.Substring(0, path.Length - 1);

            if (path.StartsWith("/"))
                path = path.Substring(1, path.Length - 1);

            if (path == "")
                return location;

            string[] segments = path.Split('/');

            var operationResult = await LiveClient.GetAsync(location + "/files?filter=folders,albums");
            List<object> data = (List<object>)operationResult.Result["data"];
            foreach (IDictionary<string, object> content in data)
            {
                if (string.Equals(content["name"], segments[0]))
                {
                    if (segments.Length > 1)
                    {
                        return await CreateFolderPathAsync(content["id"].ToString(), reducePath(segments));                
                    }
                    else
                        return content["id"].ToString();
                }
            }

            foreach (var split in segments)
            {
                location = await CreateFolderAsync(location, split);

                if (location == null)
                    return null;
            }

            return location;
        }

        private string reducePath(string[] segments)
        {
            string path = string.Empty;

            if (segments.Length == 1)
                return segments[0];

            else
                for (int i = 1; i < segments.Length; i++)
                {
                    path += segments[i];
                    if (i != segments.Length - 1)
                        path += "/";
                }

            return path;
        }

        public async Task<bool> UploadAsync(string path, string name, Stream stream)
        {
            try
            {
                LiveOperationResult result = await LiveClient.UploadAsync(path, name, stream, OverwriteOption.Overwrite);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Stream> DownloadAsync(string file)
        {
            try
            {
                LiveDownloadOperationResult result = await LiveClient.DownloadAsync(file);
                return result.Stream;
            }
            catch
            { 
                return null;
            }
        }

        public async Task<string> GetFolderLocationAsync(string location, string path)
        {
            if (path.EndsWith("/"))
                path = path.Substring(0, path.Length - 1);

            if (path.StartsWith("/"))
                path = path.Substring(1, path.Length - 1);

            if (path == "")
                return location;

            string[] segments = path.Split('/');

            var operationResult = await LiveClient.GetAsync(location + "/files");
            List<object> data = (List<object>)operationResult.Result["data"];
            foreach (IDictionary<string, object> content in data)
            {
                if (string.Equals(content["name"], segments[0]))
                {
                    if (segments.Length > 1)
                    {
                        return await GetFolderLocationAsync(content["id"].ToString(), reducePath(segments));             
                    }
                    else
                        return content["id"].ToString();
                }
            }

            return null;
        }

        public async Task<bool> DownloadRecursivly(string location, string targetStartPath)
        {
            dynamic itemList = await GetFileListAsync(location);

            if (itemList == null)
                return false;

            foreach (dynamic item in itemList)
            {
                var name = (string)item.name.ToString();

                if (item.type == "file" || item.type == "photo")
                {
                    var fileStream = await DownloadAsync(item.id + "/content");

                    if (fileStream == null)
                        return false;
                    
                    if (!StorageHelper.SaveFileFromStream(string.Format("{0}/{1}", targetStartPath, name), fileStream))
                    {
                        return false;
                    }
                    
                }
                else if (item.type == "folder")
                {
                    if (!await DownloadRecursivly(item.id, string.Format("{0}/{1}", targetStartPath, item.name)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public async Task<dynamic> GetFileListAsync(string location)
        {
            try
            {
                var operationResult = await LiveClient.GetAsync(location + "/files");
                return operationResult.Result["data"];
            }
            catch
            {
                return null;
            }
        }

        public async Task<dynamic> GetFolderListAsync(string location)
        {
            try
            {
                var operationResult = await LiveClient.GetAsync(location + "/files?filter=folders,albums");
                return operationResult.Result["data"];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/de-de/library/live/jj680723
        /// Getting a user's total and remaining OneDrive storage quota To get info about a user's 
        /// available and unused storage space in OneDrive, make a GET call to /me/skydrive/quota 
        /// or /USER_ID/skydrive/quota, where USER_ID is the signed-in user's ID.
        /// 
        /// JSON -> available
        /// </summary>
        /// <returns>Returns remaining quota in bytes.</returns>
        public async Task<long> RemainingStorageQuotaAsync()
        {
            long quota = -1;

            if (AuthClient.Session != null && LiveClient != null)
            {
                try
                {
                    var result = await LiveClient.GetAsync(ONEDRIVE_ROOT + "/quota");
                    if (result != null)
                    {
                        quota = (long)result.Result["available"];
                    }
                }
                catch (Exception)
                {
                    return -1;
                }
            }

            return quota;
        }

        /// <summary>
        /// http://msdn.microsoft.com/de-de/library/live/jj680723
        /// Getting a user's total and remaining OneDrive storage quota To get info about a user's 
        /// available and unused storage space in OneDrive, make a GET call to /me/skydrive/quota 
        /// or /USER_ID/skydrive/quota, where USER_ID is the signed-in user's ID.
        /// 
        /// JSON -> quota
        /// </summary>
        /// <returns>Returns remaining quota in bytes.</returns>
        public async Task<long> TotalStorageQuotaAsync()
        {
            long quota = -1;

            if (AuthClient.Session != null && LiveClient != null)
            {
                try
                {
                    var result = await LiveClient.GetAsync(ONEDRIVE_ROOT + "/quota");
                    if (result != null)
                    {
                        quota = (long)result.Result["quota"];
                    }
                }
                catch (Exception)
                {
                    return -1;
                }
            }

            return quota;
        }

        public static OneDriveManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OneDriveManager();
                }
                return instance;
            }
        }

    }
}
