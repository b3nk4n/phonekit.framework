using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PhoneKit.Framework.Conversion
{
    /// <summary>
    /// Converts a color to a SolidColorBrush.
    /// </summary>
    public class ColorToBrushConverter : IValueConverter
    {
        /// <summary>
        /// Converts a Color to a SolidColorBrush.
        /// </summary>
        /// <param name="value">The color to convert.</param>
        /// <returns>The converted SolidColorBrush</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Color)
            {
                var color = (Color)value;
                return new SolidColorBrush(color);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var solidColor = value as SolidColorBrush;
            if (solidColor != null)
            {
                return solidColor.Color;
            }
            return Colors.Transparent;
        }
    }
}
