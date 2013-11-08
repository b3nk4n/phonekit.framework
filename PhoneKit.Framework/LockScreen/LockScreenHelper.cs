using System;
using System.Threading.Tasks;
using Windows.Phone.System.UserProfile;

namespace PhoneKit.Framework.LockScreen
{
    /// <summary>
    /// Helper class for lock screen services.
    /// </summary>
    public static class LockScreenHelper
    {
        /// <summary>
        /// Requests for the lock screen and sets the image.
        /// </summary>
        /// <param name="imageUri">The lock key.</param>
        public static async void SetLockScreenImage(Uri imageUri)
        {
            // verify lock screen access is permitted
            if (await VerifyAccessAsync())
            {
                // set the lock screen image
                Windows.Phone.System.UserProfile.LockScreen.SetImageUri(imageUri);
            }
        }

        /// <summary>
        /// Verifies and request the lock screen access.
        /// </summary>
        /// <returns>Returns true, if lock screen access is permitted, else false.</returns>
        public static async Task<bool> VerifyAccessAsync()
        {
            if (LockScreenManager.IsProvidedByCurrentApplication)
                return true;

            // ask for permissions
            var permission = await LockScreenManager.RequestAccessAsync();

            if (permission == LockScreenRequestResult.Granted)
                return true;
            else
                return false;
        }
    }
}
