using PhoneKit.Framework.Net;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Phone.System.UserProfile;
using UserProfile = Windows.Phone.System.UserProfile;

namespace PhoneKit.Framework.LockScreen
{
    /// <summary>
    /// Helper class for lock screen services.
    /// </summary>
    public static class LockScreenHelper
    {
        #region Members

        /// <summary>
        /// The local resource scheme.
        /// </summary>
        public const string APPX_SCHEME = "ms-appx://";

        /// <summary>
        /// The download manager.
        /// </summary>
        private static readonly DownloadManager _downloadManager = new DownloadManager(
            DownloadManager.SHARED_SHELL_CONTENT_BASE_PATH,
            DownloadStorageLocation.LocalAppData);

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the lock screen image.
        /// </summary>
        /// <remarks>Remark, that the lock screen access must be requested before.</remarks>
        /// <param name="imageUri">The lock key.</param>
        public static async void SetLockScreenImage(Uri imageUri)
        {
            // verify lock screen access is permitted
            if (HasAccess())
            {
                Uri sourceUri;
                if (_downloadManager.IsWebFile(imageUri))
                {
                    Uri previousLockScreenImageUri = null;

                    try
                    {
                        // try to get the previous lockscreen image, if this app is the owner.
                        previousLockScreenImageUri = UserProfile.LockScreen.GetImageUri();
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Debug.WriteLine("Setting lockscreen image failed caused by unauthorized acces with error: "
                            + ex.Message);
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine("Setting lockscreen failed with error: " + ex.Message);
                    }

                    sourceUri = await _downloadManager.LoadFileAsync(imageUri, previousLockScreenImageUri);
                }
                else
                {
                    if (!imageUri.OriginalString.StartsWith(APPX_SCHEME))
                        sourceUri = new Uri(APPX_SCHEME + imageUri.OriginalString, UriKind.Absolute);
                    else
                        sourceUri = new Uri(imageUri.OriginalString, UriKind.Absolute);
                }

                // set the lock screen image
                UserProfile.LockScreen.SetImageUri(sourceUri);
            }
        }

        /// <summary>
        /// Verifies and request the lock screen access.
        /// </summary>
        /// <remarks>
        /// Remark that requesting the lock screen access in a background task is not allowed.
        /// </remarks>
        /// <returns>Returns true, if lock screen access is permitted, else false.</returns>
        public static async Task<bool> VerifyAccessAsync()
        {
            if (HasAccess())
                return true;

            // ask for permissions
            var permission = await LockScreenManager.RequestAccessAsync();

            if (permission == LockScreenRequestResult.Granted)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks the lock screen access.
        /// </summary>
        /// <returns>Returns true, if the application has lock screen access, else false.</returns>
        public static bool HasAccess()
        {
            return LockScreenManager.IsProvidedByCurrentApplication;
        }

        #endregion
    }
}
