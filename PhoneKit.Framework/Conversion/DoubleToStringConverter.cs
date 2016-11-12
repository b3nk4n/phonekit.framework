using System;
using System.Globalization;
using System.Windows.Data;

namespace PhoneKit.Framework.Conversion
{
    /// <summary>
    /// Value converter that converts a double value into a string and vice versa.
    /// </summary>
    public sealed class DoubleToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts the double value into a string.
        /// </summary>
        /// <param name="value">The double value.</param>
        /// <param name="targetType">The target conversion type.</param>
        /// <param name="parameter">The string parameter saying how may digits after the comma.</param>
        /// <param name="culture">The culture information.</param>
        /// <returns>The negated value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                var val = (double)value;

                if (parameter == null)
                    return value.ToString();

                double rounded = Math.Round(val, int.Parse((string)parameter));
                return rounded.ToString();
            }

            return 0.0;
        }

        /// <summary>
        /// Converts a string into a double value.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The not supported parameter.</param>
        /// <param name="culture">The culture</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                return Double.Parse((string)value);
            }

            return 0.0;
        }
    }
}
