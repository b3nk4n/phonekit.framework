using System;
using System.Globalization;
using System.Windows.Data;

namespace PhoneKit.Framework.Conversion
{
    /// <summary>
    /// Converter to convert a DateTime object into a short date format.
    /// </summary>
    public class ShortDateConverter : IValueConverter
    {
        /// <summary>
        /// Converts the object to a string.
        /// </summary>
        /// <param name="value">The datetime value.</param>
        /// <param name="targetType">The target conversion type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture information.</param>
        /// <returns>The short date time.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
                return ((DateTime) value).ToShortDateString();
            return string.Empty;
        }

        /// <summary>
        /// Backwards conversion is not supported.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
