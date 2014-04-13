using PhoneKit.Framework.Core.Net;
using PhoneKit.Framework.Core.Storage;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Phone.System.UserProfile;
using UserProfile = Windows.Phone.System.UserProfile;

namespace PhoneKit.Framework.Core.LockScreen
{
    /// <summary>
    /// Helper class for lock screen services.
    /// </summary>
    public static class LockScreenHelper
    {
        #region Members

        /// <summary>
        /// The download manager.
        /// </summary>
        private static readonly DownloadManager _downloadManager = new DownloadManager(
            "/lockscreen/",
            DownloadStorageLocation.LocalAppData);

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the lock screen image.
        /// </summary>
        /// <remarks>Remark, that the lock screen access must be requested before.</remarks>
        /// <param name="imageUri">The lock key.</param>
        public static void SetLockScreenImage(Uri imageUri)
        {
            SetLockScreenImage(imageUri, false);
        }

        /// <summary>
        /// Sets the lock screen image.
        /// </summary>
        /// <remarks>Remark, that the lock screen access must be requested before.</remarks>
        /// <param name="imageUri">The lock key.</param>
        /// <param name="isLocalImage">Whether the image is local (LOCAL APPDATA/ISOSTORE) or an app resource (APPX).</param>
        public static async void SetLockScreenImage(Uri imageUri, bool isLocalImage)
        {
            try
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
                            Debug.WriteLine("Retrieving previous lock screen image failed caused by unauthorized acces with error: "
                                + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Retrieving previous lock screen image failed with error: " + ex.Message);
                        }

                        sourceUri = await _downloadManager.LoadFileAsync(imageUri, previousLockScreenImageUri);
                    }
                    else
                    {
                        if (!imageUri.OriginalString.StartsWith(StorageHelper.APPX_SCHEME) ||
                            !imageUri.OriginalString.StartsWith(StorageHelper.APPDATA_LOCAL_SCHEME))
                        {
                            if (isLocalImage)
                            {
                                // ensure to remove the isostore prefix
                                if (imageUri.OriginalString.StartsWith(StorageHelper.ISTORAGE_SCHEME))
                                    sourceUri = new Uri(StorageHelper.APPDATA_LOCAL_SCHEME + imageUri.AbsolutePath, UriKind.Absolute);
                                else
                                    sourceUri = new Uri(StorageHelper.APPDATA_LOCAL_SCHEME + imageUri.OriginalString, UriKind.Absolute);
                            }
                            else
                                sourceUri = new Uri(StorageHelper.APPX_SCHEME + imageUri.OriginalString, UriKind.Absolute);
                        }
                        else
                            sourceUri = new Uri(imageUri.OriginalString, UriKind.Absolute);
                    }

                    // set the lock screen image
                    UserProfile.LockScreen.SetImageUri(sourceUri);
                }
            }
            catch (Exception e)
            {
                /* note: the whole method is wrapped, because on time there was an exception with the following error:
                <trace>
                   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
                   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
                   at System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
                   at PhoneKit.Framework.Core.LockScreen.LockScreenHelper.<VerifyAccessAsync>d__5.MoveNext()
                --- End of stack trace from previous location where exception was thrown ---
                   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
                   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
                   at System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
                   at PocketBrain.App.ViewModel.NoteListViewModel.<<.ctor>b__0>d__c.MoveNext()
                --- End of stack trace from previous location where exception was thrown ---
                   at System.Runtime.CompilerServices.AsyncMethodBuilderCore.<ThrowAsync>b__0(Object state)
                </trace>*/
                Debug.WriteLine("Setting the lock screen failed with error: " + e.Message);
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

        /// <summary>
        /// Clears the used storage of downloaded files.
        /// </summary>
        public static void ClearStorage()
        {
            _downloadManager.Clear();
        }

        #endregion
    }
}
