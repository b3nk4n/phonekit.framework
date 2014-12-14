using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PhoneKit.Framework.Conversion
{
    /// <summary>
    /// Value converter that translates true to "1", else "2" in a parameter like "1,2".
    /// </summary>
    public sealed class BooleanToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean to a string given by the parameter value.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        /// <param name="targetType">The target conversion type.</param>
        /// <param name="parameter">The parameter string in the form "x,y".</param>
        /// <param name="culture">The culture information.</param>
        /// <returns>The converted string value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var paramString = parameter as string;

            if (paramString == null)
                return string.Empty;

            var splitted = paramString.Split(',');

            if (splitted.Length == 1)
                return splitted[0];

            return (value is bool && (bool)value) ? splitted[0] : splitted[1];
        }

        /// <summary>
        /// This conversion method is not supported for this type.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
