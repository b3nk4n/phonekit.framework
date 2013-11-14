using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PhoneKit.Framework.Conversion
{
    /// <summary>
    /// Value converter that creates a clock image from a time value.
    /// Reference the System.Windows.Media namespace.
    /// You can bind the result to a Path element.
    /// </summary>
    public sealed class TimeToShapeConverter : IValueConverter
    {
        /// <summary>
        /// Converts a datetime to a visual shape.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        /// <param name="targetType">The target conversion type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture information.</param>
        /// <returns>The time shape.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dt = (DateTime)value;
            GeometryGroup coll = new GeometryGroup();
            EllipseGeometry ell = new EllipseGeometry();
            ell.Center = new Point(55, 55);
            ell.RadiusX = ell.RadiusY = 60;
            coll.Children.Add(ell);
            LineGeometry hour = new LineGeometry();
            double deg = (dt.Hour % 12) * Math.PI / 6;
            hour.StartPoint = ell.Center;
            hour.EndPoint = new Point(55 + Math.Sin(deg) * 35, 55 - Math.Cos(deg) * 35);
            coll.Children.Add(hour);
            LineGeometry minute = new LineGeometry();
            minute.StartPoint = ell.Center;
            deg = dt.Minute * Math.PI / 30;
            minute.EndPoint = new Point(55 + Math.Sin(deg) * 50, 55 - Math.Cos(deg) * 50);
            coll.Children.Add(minute);
            return coll;
        }

        /// <summary>
        /// Backward convesion is not supported.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
