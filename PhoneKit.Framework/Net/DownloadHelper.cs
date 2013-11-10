using PhoneKit.Framework.Storage;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Threading.Tasks;

namespace PhoneKit.Framework.Net
{
    public enum DownloadStorageLocation
    {
        IsolatedStorage,
        LocalAppData
    }
    /// <summary>
    /// Helper class for downloading items.
    /// </summary>
    public class DownloadManager
    {
        #region Members

        /// <summary>
        /// The download location.
        /// </summary>
        private DownloadStorageLocation _downloadStorage;

        /// <summary>
        /// The base folder path.
        /// </summary>
        private string _baseFolderPath;

        /// <summary>
        /// The isolated storage scheme.
        /// </summary>
        public const string ISTORAGE_SCHEME = "isostore://";

        /// <summary>
        /// The local appdata scheme.
        /// </summary>
        public const string APPDATA_SCHEME = "ms-appdata:///local";

        /// <summary>
        /// The web http scheme.
        /// </summary>
        public const string HTTP_SCHEME = "http://";

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new DownloadManager instance with isolated storage root as default.
        /// </summary>
        public DownloadManager()
            : this("/", DownloadStorageLocation.IsolatedStorage)
        { }

        /// <summary>
        /// Creates a new DownloadManager instance.
        /// </summary>
        /// <param name="baseFolderPath">The base folder path for the downloaded files.</param>
        /// <param name="downloadStorage">The download storage location.</param>
        public DownloadManager(string baseFolderPath, DownloadStorageLocation downloadStorage)
        {
            BaseFolderPath = baseFolderPath;
            DownloadStorage = downloadStorage;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Verifies whether the file is from the web or not.
        /// </summary>
        /// <param name="fileUri">The file URI.</param>
        /// <returns>Returns true, if the file if from web, else false.</returns>
        public bool IsWebFile(Uri fileUri)
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
        /// <param name="mustBeDifferentFromLocal">
        /// Makes the file name unique using A/B switching, which is required for example for lockscreen images.
        /// </param>
        /// <returns>
        /// The local file path of the downloaded file, eighter from isolated storage or from resources.
        /// </returns>
        public async Task<Uri> LoadFileAsync(Uri fileUri, Uri mustBeDifferentFromLocal=null)
        {
            try
            {
                // check if it is an image from web
                if (IsWebFile(fileUri))
                {
                    string localPath;
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileUri.LocalPath.Replace('/', '_').Replace('\\', '_'));
                    string extension = Path.GetExtension(fileUri.LocalPath);

                    // verify unique image file name
                    localPath = VerifyUniqueFileName(mustBeDifferentFromLocal, fileNameWithoutExt, extension);

                    return await LoadFileAsync(fileUri, localPath, DownloadStorage);
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
        /// Clears all downloaded files.
        /// </summary>
        /// <remarks>
        /// If the folder is shared with another download manager, remark that ALL files are deleted.
        /// </remarks>
        public void Clear()
        {
            IsolatedStorageHelper.TryDeleteAllDirectoryFiles(BaseFolderPath);
        }

        #endregion

        #region Private Methods

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
        private string VerifyUniqueFileName(Uri mustBeDifferentFromLocal, string fileNameWithoutExt, string extension)
        {
            // verify if a unique name is required
            if (mustBeDifferentFromLocal != null)
            {
                string oldFileName = Path.GetFileNameWithoutExtension(mustBeDifferentFromLocal.LocalPath);

                if (oldFileName.StartsWith(fileNameWithoutExt))
                {
                    // toggle suffix
                    if (Path.GetFileNameWithoutExtension(mustBeDifferentFromLocal.LocalPath).EndsWith("_A"))
                        return string.Format("{0}{1}_B{2}", BaseFolderPath, fileNameWithoutExt, extension);
                    else
                        return string.Format("{0}{1}_A{2}", BaseFolderPath, fileNameWithoutExt, extension);
                }
            }

            // giva all new lockscreen images with an '_A' suffix
            return string.Format("{0}{1}_A{2}", BaseFolderPath, fileNameWithoutExt, extension);
        }

        /// <summary>
        /// Downloads a file from the web and stores it in isolated storage.
        /// </summary>
        /// <param name="webUri">The web URI.</param>
        /// <param name="localPath">The local desired path in isolated storage.</param>
        /// <param name="downloadStorage">The local download storage location.</param>
        /// <returns>The local Uri in isolated storage of the downloaded file.</returns>
        private async Task<Uri> LoadFileAsync(Uri webUri, string localPath, DownloadStorageLocation downloadStorage)
        {
            var request = WebRequest.CreateHttp(webUri);
            var task = Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse,
                request.EndGetResponse,
                null);
            try
            {
                await task.ContinueWith(t =>
                {
                    var stream = task.Result.GetResponseStream();

                    if (!IsolatedStorageHelper.SaveFileFromStream(localPath, stream))
                    {
                        Debug.WriteLine("Saving the downloaded file not successful");
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Download file with tile data failed with error: " + ex.Message);
            }

            // select required scheme prefix
            string scheme = downloadStorage == DownloadStorageLocation.IsolatedStorage ? ISTORAGE_SCHEME : APPDATA_SCHEME;

            return new Uri(scheme + localPath, UriKind.RelativeOrAbsolute);
        }


        #endregion

        #region Properties

        /// <summary>
        /// Gets the bbase folder path.
        /// </summary>
        public string BaseFolderPath
        {
            get
            {
                return _baseFolderPath;
            }
            private set
            {
                _baseFolderPath = value;
                if (!_baseFolderPath.EndsWith("/"))
                    _baseFolderPath += "/";
                if (!_baseFolderPath.StartsWith("/"))
                    _baseFolderPath = "/" + _baseFolderPath;
            }
        }

        /// <summary>
        /// Gets the download location.
        /// </summary>
        public DownloadStorageLocation DownloadStorage
        {
            get
            {
                return _downloadStorage;
            }
            set
            {
                _downloadStorage = value;
            }
        }

        #endregion
    }
}
