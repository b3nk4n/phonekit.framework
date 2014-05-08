using System.Windows;
using System.Windows.Media;

namespace PhoneKit.Framework.Core.Themeing
{
    /// <summary>
    /// Helper class for phone theme management.
    /// </summary>
    public static class PhoneThemeHelper
    {
        /// <summary>
        /// Overrides the given resource key for color and brush, using the naming pattern: *Color and *Name.
        /// </summary>
        /// <remarks>
        /// This is NOT working with system resources.
        /// </remarks>
        /// <param name="keyPrefix">The beginning of the key.</param>
        /// <param name="color">The color to override.</param>
        public static void OverridePhoneBackground(string keyPrefix, Color color)
        {
            string colorKey = keyPrefix + "Color";
            string brushKey = keyPrefix + "Brush";

            Application.Current.Resources.Remove(colorKey);
            Application.Current.Resources.Add(colorKey, color);
            Application.Current.Resources.Remove(brushKey);
            Application.Current.Resources.Add(brushKey, new SolidColorBrush(color));
        }

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
