using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Threading.Tasks;

namespace PhoneKit.Framework.Net
{
    public enum DownloadLocation
    {
        IsolatedStorage,
        LocalAppData
    }
    /// <summary>
    /// Helper class for downloading items.
    /// </summary>
    public static class DownloadHelper
    {
        /// <summary>
        /// The isolated storage scheme.
        /// </summary>
        public const string ISTORAGE_SCHEME = "isostore:";

        /// <summary>
        /// The local appdata scheme.
        /// </summary>
        public const string APPDATA_SCHEME = "ms-appdata:///Local";

        /// <summary>
        /// The web http scheme.
        /// </summary>
        public const string HTTP_SCHEME = "http://";

        /// <summary>
        /// Verifies whether the file is from the web or not.
        /// </summary>
        /// <param name="fileUri">The file URI.</param>
        /// <returns>Returns true, if the file if from web, else false.</returns>
        public static bool IsWebFile(Uri fileUri)
        {
            if (fileUri == null)
                return false;

            return fileUri.OriginalString.StartsWith(HTTP_SCHEME);
        }

        /// <summary>
        /// Gets the local image path, so it downloads the image if the file is from the web.
        /// </summary>
        /// <param name="fileUri">
        /// The image of the tile or if empty, the one of the life tile data is used.
        /// </param>
        /// <param name="downloadLocation">The local download location.</param>
        /// <param name="mustBeDifferentFromLocal">
        /// Makes the file name unique using A/B switching, which is required for example for lockscreen images.
        /// </param>
        /// <returns>
        /// The local file path of the downloaded file, eighter from isolated storage or from resources.
        /// </returns>
        public static async Task<Uri> LoadFileAsync(Uri fileUri, DownloadLocation downloadLocation, Uri mustBeDifferentFromLocal=null)
        {
            try
            {
                // check if it is an image from web
                if (IsWebFile(fileUri))
                {
                    string localPath;
                    string basePath = "/shared/shellcontent/";
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileUri.LocalPath.Replace('/', '_').Replace('\\', '_'));
                    string extension = Path.GetExtension(fileUri.LocalPath);

                    // verify unique image file name
                    localPath = VerifyUniqueFileName(mustBeDifferentFromLocal, basePath, fileNameWithoutExt, extension);

                    return await DownloadHelper.LoadFileAsync(fileUri, localPath, downloadLocation);
                }

                // get image from resources
                return fileUri;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Getting the tile image path failed with error: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Verifies that the file name is unique by A/B-suffixing.
        /// </summary>
        /// <remarks>
        /// Required for example for lockscreen images.
        /// </remarks>
        /// <param name="mustBeDifferentFromLocal">The previewsFileName.</param>
        /// <param name="basePath">The base path.</param>
        /// <param name="fileNameWithoutExt">The file name without extension.</param>
        /// <param name="extension">The file extension.</param>
        /// <returns></returns>
        private static string VerifyUniqueFileName(Uri mustBeDifferentFromLocal, string basePath,
            string fileNameWithoutExt, string extension)
        {
            // verify if a unique name is required
            if (mustBeDifferentFromLocal != null)
            {
                string oldFileName = Path.GetFileNameWithoutExtension(mustBeDifferentFromLocal.LocalPath);

                if (oldFileName.StartsWith(fileNameWithoutExt))
                {
                    // toggle suffix
                    if (Path.GetFileNameWithoutExtension(mustBeDifferentFromLocal.LocalPath).EndsWith("_A"))
                        return string.Format("{0}{1}_B{2}", basePath, fileNameWithoutExt, extension);
                    else
                        return string.Format("{0}{1}_A{2}", basePath, fileNameWithoutExt, extension);
                }
            }

            // giva all new lockscreen images with an '_A' suffix
            return string.Format("{0}{1}_A{2}", basePath, fileNameWithoutExt, extension);
        }

        /// <summary>
        /// Downloads a file from the web and stores it in isolated storage.
        /// </summary>
        /// <param name="webUri">The web URI.</param>
        /// <param name="localPath">The local desired path in isolated storage.</param>
        /// <param name="downloadLocation">The local download location.</param>
        /// <returns>The local Uri in isolated storage of the downloaded file.</returns>
        private static async Task<Uri> LoadFileAsync(Uri webUri, string localPath, DownloadLocation downloadLocation)
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

            // select required scheme prefix
            string scheme = downloadLocation == DownloadLocation.IsolatedStorage ? ISTORAGE_SCHEME : APPDATA_SCHEME;

            return new Uri(scheme + localPath, UriKind.RelativeOrAbsolute);
        }
    }
}
