using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhoneKit.Framework.Core.Graphics
{
    /// <summary>
    /// Helper class for rendering images.
    /// </summary>
    public static class GraphicsHelper
    {
        /// <summary>
        /// Creats an image with the dimension of the give element.
        /// </summary>
        /// <param name="element">The framework element.</param>
        /// <returns>The renderable image.</returns>
        public static WriteableBitmap Create(FrameworkElement element)
        {
            return Create(element, (int)element.Width, (int)element.Height);
        }

        /// <summary>
        /// Creats an image.
        /// </summary>
        /// <param name="element">The framework element.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>The renderable image.</returns>
        public static WriteableBitmap Create(FrameworkElement element, int width, int height)
        {
            var wbmp = new WriteableBitmap(width, height);

            // Force the content to layout itself properly
            element.UpdateLayout();
            element.Measure(new Size(width, height));
            element.UpdateLayout();
            element.Arrange(new Rect(0, 0, width, height));


            wbmp.Render(element, null);
            wbmp.Invalidate();

            return wbmp;
        }

        /// <summary>
        /// Renders content to an renderable image.
        /// </summary>
        /// <param name="bitmap">The existing renderable image data.</param>
        /// <param name="textBlock">The text to render.</param>
        public static void RenderString(WriteableBitmap bitmap, TextBlock textBlock)
        {
            bitmap.Render(textBlock, null);
            bitmap.Invalidate();
        }

        /// <summary>
        /// Renders content to an renderable image.
        /// </summary>
        /// <param name="bitmap">The existing renderable image data.</param>
        /// <param name="image">The image to render.</param>
        public static void RenderImage(WriteableBitmap bitmap, Image image)
        {
            bitmap.Render(image, null);
            bitmap.Invalidate();
        }


        /// <summary>
        /// Renders content to an renderable image.
        /// </summary>
        /// <param name="bitmap">The existing renderable image data.</param>
        /// <param name="rectangle">The rectangle to render.</param>
        public static void RenderRectangle(WriteableBitmap bitmap, Rectangle rectangle)
        {
            bitmap.Render(rectangle, null);
            bitmap.Invalidate();
        }

        /// <summary>
        /// Renders content to an renderable image.
        /// </summary>
        /// <param name="bitmap">The existing renderable image data.</param>
        /// <param name="ellipse">The ellipse to render.</param>
        public static void RenderEllipse(WriteableBitmap bitmap, Ellipse ellipse)
        {
            bitmap.Render(ellipse, null);
            bitmap.Invalidate();
        }

        /// <summary>
        /// Renders content to an renderable image.
        /// </summary>
        /// <param name="bitmap">The existing renderable image data.</param>
        /// <param name="line">The line to render.</param>
        public static void RenderLine(WriteableBitmap bitmap, Line line)
        {
            bitmap.Render(line, null);
            bitmap.Invalidate();
        }

        /// <summary>
        /// Renders content to an renderable image.
        /// </summary>
        /// <param name="bitmap">The existing renderable image data.</param>
        /// <param name="path">The path to render.</param>
        public static void RenderPath(WriteableBitmap bitmap, Path path)
        {
            bitmap.Render(path, null);
            bitmap.Invalidate();
        }

        /// <summary>
        /// Renders content to an renderable image.
        /// </summary>
        /// <param name="bitmap">The existing renderable image data.</param>
        /// <param name="polygone">The polygone to render.</param>
        public static void RenderPolygon(WriteableBitmap bitmap, Polygon polygon)
        {
            bitmap.Render(polygon, null);
            bitmap.Invalidate();
        }

        /// <summary>
        /// Renders content to an renderable image.
        /// </summary>
        /// <param name="bitmap">The existing renderable image data.</param>
        /// <param name="polygone">The polygone to render.</param>
        public static void RenderPolyline(WriteableBitmap bitmap, Polyline polyline)
        {
            bitmap.Render(polyline, null);
            bitmap.Invalidate();
        }

        /// <summary>
        /// Renders content to an renderable image.
        /// </summary>
        /// <param name="bitmap">The existing renderable image data.</param>
        /// <param name="shape">The shape to render.</param>
        public static void RenderShape(WriteableBitmap bitmap, Shape shape)
        {
            bitmap.Render(shape, null);
            bitmap.Invalidate();
        }
    }
}
