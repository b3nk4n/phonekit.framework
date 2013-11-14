using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PhoneKit.Framework.Conversion
{
    /// <summary>
    /// Value converter that translates true to <see cref="Visibility.Visible"/> and false to
    /// <see cref="Visibility.Collapsed"/>.
    /// </summary>
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean to a visiblity value.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        /// <param name="targetType">The target conversion type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture information.</param>
        /// <returns>The converted visibility value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a visibility to a boolean value.
        /// </summary>
        /// <param name="value">The datetime value.</param>
        /// <param name="targetType">The target conversion type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture information.</param>
        /// <returns>The converted boolean value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }
}
