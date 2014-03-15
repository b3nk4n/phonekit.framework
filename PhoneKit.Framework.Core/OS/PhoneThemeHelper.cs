using System.Windows;

namespace PhoneKit.Framework.Core.OS
{
    /// <summary>
    /// Helper class for phone theme management.
    /// </summary>
    public static class PhoneThemeHelper
    {
        /// <summary>
        /// Indicates whether the dark theme is active.
        /// </summary>
        /// <returns>Returns true if the dark theme is active.</returns>
        public static bool IsDarkThemeActive
        {
            get
            {
                Visibility darkBackgroundVisibility =
                (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"];

                return darkBackgroundVisibility == Visibility.Visible;
            }
        }

        /// <summary>
        /// Indicates whether the light theme is active.
        /// </summary>
        /// <returns>Returns true if the light theme is active.</returns>
        public static bool IsLightThemeActive
        {
            get
            {
                return !IsDarkThemeActive;
            }
        }
    }
}
