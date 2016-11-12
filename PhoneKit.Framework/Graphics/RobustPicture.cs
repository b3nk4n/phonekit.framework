using ExifLib;
using Microsoft.Phone;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace PhoneKit.Framework.Graphics
{
    /// <summary>
    /// Robust picture, which ensures that pictures are always oriented correctly and downscales huge
    /// pictures to reduce the probability of an OutOfMemeoryException.
    /// This is due to the bug in all new Lumia phones (930, ...), where the picture data is stored
    /// in a different propotion.
    /// <seealso cref="http://sylvana.net/jpegcrop/exif_orientation.html"/>
    /// </summary>
    public class RobustPicture : IDisposable
    {
        public const ushort ORIENTATION_NORMAL = 1;
        public const ushort ORIENTATION_ABNORMAL_90 = 6;
        public const ushort ORIENTATION_ABNORMAL_180 = 3;
        public const ushort ORIENTATION_ABNORMAL_270 = 8;

        /// <summary>
        /// The cached width for better performance.
        /// </summary>
        private int _cachedWidth;

        /// <summary>
        /// The cached height for better performance.
        /// </summary>
        private int _cachedHeight;

        /// <summary>
        /// Creates a orientation aware photo.
        /// </summary>
        /// <param name="picture">The picture to wrap.</param>
        public RobustPicture(Picture picture)
        {
            InternalPicture = picture;
            LoadExifData();

            UpdateCachedImageSize(picture.Width, picture.Height);
        }

        /// <summary>
        /// Updates the cached image size.
        /// </summary>
        /// <param name="width">The width</param>
        /// <param name="height">The height.</param>
        private void UpdateCachedImageSize(int width, int height)
        {
            if (IsOrientationFlipped)
            {
                _cachedHeight = width;
                _cachedWidth = height;
            }
            else
            {
                _cachedHeight = height;
                _cachedWidth = width;
            }
        }

        /// <summary>
        /// Initializes the EXIF data.
        /// </summary>
        private void LoadExifData()
        {
            ushort orientation;
            try
            {
                using (ExifReader exifReader = new ExifReader(InternalPicture.GetImage()))
                {
                    exifReader.GetTagValue<ushort>(ExifTags.Orientation, out orientation);
                }
            }
            catch (Exception)
            {
                orientation = ORIENTATION_NORMAL;
            }
            
            ExifOrientation = orientation;
        }

        /// <summary>
        /// Gets the thumbnail image with low resolution.
        /// </summary>
        public ImageSource ThumbnailImage
        {
            get
            {
                return PictureDecoder.DecodeJpeg(InternalPicture.GetThumbnail());
            }
        }

        /// <summary>
        /// Gets the preview image with medium or full resolution.
        /// </summary>
        public ImageSource PreviewImage
        {
            get
            {
                var image = PictureDecoder.DecodeJpeg(InternalPicture.GetPreviewImage());
                image = UpdateRotation(image);
                return image;
            }
        }

        /// <summary>
        /// Gets the image in full resolution.
        /// </summary>
        public ImageSource Image
        {
            get
            {
                WriteableBitmap image;
                image = GetDecodedImage();
                image = UpdateRotation(image);
                return image;
            }
        }

        /// <summary>
        /// Gets the decoded image.
        /// </summary>
        /// <returns>The decoded image.</returns>
        private WriteableBitmap GetDecodedImage()
        {
            WriteableBitmap image;
            if (IsOutOfMemoryPossible())
            {
                image = PictureDecoder.DecodeJpeg(InternalPicture.GetPreviewImage());
            }
            else
            {
                image = PictureDecoder.DecodeJpeg(InternalPicture.GetImage());
            }

            // FIX 31.08: change orientation of parameters, because on some devices the preview image has correct height/width, where the full image has swapped values!
            //UpdateCachedImageSize(image.PixelWidth, image.PixelHeight); 
            var w = image.PixelWidth;
            var h = image.PixelHeight;

            if (InternalPicture.Width > InternalPicture.Height && h > w ||
                InternalPicture.Width < InternalPicture.Height && h < w)
            {
                w = image.PixelHeight;
                h = image.PixelWidth;
            }

            UpdateCachedImageSize(w, h);

            return image;
        }


        /// <summary>
        /// Updates the photo roation based on the EXIF data.
        /// </summary>
        /// <param name="image">The photo data.</param>
        /// <returns>The correctly rotated photo data.</returns>
        private WriteableBitmap UpdateRotation(WriteableBitmap image)
        {
            if (ExifOrientation == ORIENTATION_ABNORMAL_90)
            {
                image = image.Rotate(90);
            }
            else if (ExifOrientation == ORIENTATION_ABNORMAL_180)
            {
                image = image.Rotate(180);
            }
            else if (ExifOrientation == ORIENTATION_ABNORMAL_270)
            {
                image = image.Rotate(270);
            }
            return image;
        }

        /// <summary>
        /// Indicates whether the image is too huge and can produce a OutOfMemeoryException.
        /// </summary>
        /// <returns>True, if OutOfMemoryException is possible.</returns>
        private bool IsOutOfMemoryPossible()
        {
            return InternalPicture.Width > 3600 || InternalPicture.Height > 3600;
        }

        /// <summary>
        /// Disposes the image.
        /// </summary>
        public void Dispose()
        {
            if (!InternalPicture.IsDisposed)
                InternalPicture.Dispose();
        }

        /// <summary>
        /// Gets the internal picture data.
        /// </summary>
        public Picture InternalPicture
        {
            private set; get;
        }

        /// <summary>
        /// Gets or sets the EXIF orientation of the photo.
        /// </summary>
        public ushort ExifOrientation
        {
            get; set;
        }

        /// <summary>
        /// Gets whether the orientation is changed.
        /// </summary>
        public bool IsOrientationFlipped
        {
            get
            {
                return ExifOrientation == ORIENTATION_ABNORMAL_90 ||
                        ExifOrientation == ORIENTATION_ABNORMAL_270;
            }
        }

        /// <summary>
        /// Get the orientation neutral width.
        /// </summary>
        public int Width
        {
            get
            {
                return _cachedWidth;
            }
        }

        /// <summary>
        /// Get the orientation neutral height.
        /// </summary>
        public int Height
        {
            get
            {
                return _cachedHeight;
            }
        } 
    }
}