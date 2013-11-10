using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Threading.Tasks;

namespace PhoneKit.Framework.Net
{
    /// <summary>
    /// Helper class for downloading items.
    /// </summary>
    public static class DownloadHelper
    {
        /// <summary>
        /// Downloads a file from the web and stores it in isolated storage.
        /// </summary>
        /// <param name="webUri">The web URI.</param>
        /// <param name="localPath">The local desired path in isolated storage.</param>
        /// <returns>The local Uri in isolated storage of the downloaded file.</returns>
        public static async Task<Uri> LoadFileAsync(Uri webUri, string localPath)
        {
            var request = WebRequest.CreateHttp(webUri);
            var task = Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse,
                request.EndGetResponse,
                null);
            await task.ContinueWith(t =>
            {
                using (var responseStream = task.Result.GetResponseStream())
                {
                    using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        try
                        {
                            using (var isoStoreFile = isoStore.OpenFile(localPath,
                                FileMode.Create,
                                FileAccess.ReadWrite))
                            {
                                // store loaded data in isolated storage
                                var dataBuffer = new byte[1024];
                                while (responseStream.Read(dataBuffer, 0, dataBuffer.Length) > 0)
                                {
                                    isoStoreFile.Write(dataBuffer, 0, dataBuffer.Length);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Download file with tile data failed with error: " + ex.Message);
                        }
                    }
                }
            });

            return new Uri("isostore:" + localPath, UriKind.RelativeOrAbsolute);
        }
    }
}
