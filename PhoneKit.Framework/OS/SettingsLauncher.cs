using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneKit.Framework.OS
{
    /// <summary>
    /// Helper class for launching the settings pages.
    /// </summary>
    public static class SettingsLauncher
    {
        /// <summary>
        /// Lauchnes the airplane mode settings.
        /// </summary>
        /// <returns>Returns true if successful, else false.</returns>
        public static async Task<bool> LaunchAirplaneModeAsync()
        {
            return await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-airplanemode:"));
        }

        /// <summary>
        /// Lauchnes the bluetooth settings.
        /// </summary>
        /// <returns>Returns true if successful, else false.</returns>
        public static async Task<bool> LaunchBluetoothAsync()
        {
            return await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:"));
        }

        /// <summary>
        /// Lauchnes the cellular settings.
        /// </summary>
        /// <returns>Returns true if successful, else false.</returns>
        public static async Task<bool> LaunchCellularAsync()
        {
            return await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-cellular:"));
        }

        /// <summary>
        /// Lauchnes the email accounts settings.
        /// </summary>
        /// <returns>Returns true if successful, else false.</returns>
        public static async Task<bool> LaunchEmailAccountsAsync()
        {
            return await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-emailandaccounts:"));
        }

        /// <summary>
        /// Lauchnes the location settings.
        /// </summary>
        /// <returns>Returns true if successful, else false.</returns>
        public static async Task<bool> LaunchLocationAsync()
        {
            return await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-location:"));
        }

        /// <summary>
        /// Lauchnes the lock screen settings.
        /// </summary>
        /// <returns>Returns true if successful, else false.</returns>
        public static async Task<bool> LaunchLockScreenAsync()
        {
            return await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-lock:"));
        }

        /// <summary>
        /// Lauches the battery saver settings.
        /// </summary>
        /// <returns>Returns true if successful, else false.</returns>
        public static async Task<bool> LaunchBatterySaverAsync()
        {
            return await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-power:"));
        }

        /// <summary>
        /// Lauchnes the screen rotation settings.
        /// </summary>
        /// <remarks>
        /// Requires the GDR3 update.
        /// </remarks>
        /// <returns>Returns true if successful, else false.</returns>
        public static async Task<bool> LaunchScreenRotationAsync()
        {
            if (VersionHelper.IsPhoneGDR3)
                return await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-screenrotation:"));
            else
                return false;
        }

        /// <summary>
        /// Lauches the wifi settings.
        /// </summary>
        /// <returns>Returns true if successful, else false.</returns>
        public static async Task<bool> LaunchWifiAsync()
        {
            return await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-wifi:"));
        }
    }
}
