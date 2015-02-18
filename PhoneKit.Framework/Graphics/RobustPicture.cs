using ExifLib;
using Microsoft.Phone;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using System;
using System.Diagnostics;
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
    public class RobustPicture
    {
        public const ushort ORIENTATION_NORMAL = 1;
        public const ushort ORIENTATION_ABNORMAL_90 = 6;
        public const ushort ORIENTATION_ABNORMAL_180 = 3;
        public const ushort ORIENTATION_ABNORMAL_270 = 8;

        /// <summary>
        /// Creates a orientation aware photo.
        /// </summary>
        /// <param name="picture">The picture to wrap.</param>
        public RobustPicture(Picture picture)
        {
            InternalPicture = picture;
            LoadExifData();
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
                image = PictureDecoder.DecodeJpeg(InternalPicture.GetPreviewImage());
            else
                image = PictureDecoder.DecodeJpeg(InternalPicture.GetImage());
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
            return InternalPicture.Width > 4000 || InternalPicture.Height > 4000;
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
        /// Get the orientation neutral width.
        /// </summary>
        public int Width
        {
            get
            {
                if (ExifOrientation == ORIENTATION_ABNORMAL_90 || 
                    ExifOrientation == ORIENTATION_ABNORMAL_270)
                {
                    if (IsOutOfMemoryPossible())
                        return GetDecodedImage().PixelHeight;
                    return InternalPicture.Height;
                }

                if (IsOutOfMemoryPossible())
                    return GetDecodedImage().PixelWidth;
                return InternalPicture.Width;
            }
        }

        /// <summary>
        /// Get the orientation neutral height.
        /// </summary>
        public int Height
        {
            get
            {
                if (ExifOrientation == ORIENTATION_ABNORMAL_90 ||
                    ExifOrientation == ORIENTATION_ABNORMAL_270)
                {
                    if (IsOutOfMemoryPossible())
                        return GetDecodedImage().PixelWidth;
                    return InternalPicture.Width;
                }

                if (IsOutOfMemoryPossible())
                    return GetDecodedImage().PixelHeight;
                return InternalPicture.Height;
            }
        }
    }
}