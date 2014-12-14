using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PhoneKit.Framework.Conversion
{
    /// <summary>
    /// Value converter that translates true to 0.1, else 0.5 in a parameter like "0.1,0.5".
    /// </summary>
    public sealed class BooleanToDoubleConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean to a double given by the parameter value.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        /// <param name="targetType">The target conversion type.</param>
        /// <param name="parameter">The parameter string in the form "x,y".</param>
        /// <param name="culture">The culture information.</param>
        /// <returns>The converted double value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var paramString = parameter as string;

            if (paramString == null)
                return 0.0;

            var splitted = paramString.Split(',');

            if (splitted.Length == 1)
                return Double.Parse(splitted[0]);

            return (value is bool && (bool)value) ? Double.Parse(splitted[0], CultureInfo.InvariantCulture) : Double.Parse(splitted[1], CultureInfo.InvariantCulture);
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
