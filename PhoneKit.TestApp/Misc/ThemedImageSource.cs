using PhoneKit.Framework.Core.Themeing;

namespace PhoneKit.TestApp.Misc
{
    public class ThemedImageSource : ThemedImageSourceBase
    {
        public ThemedImageSource()
            : base("Assets/Images")
        {
        }

        /// <summary>
        /// Ges the next image path.
        /// </summary>
        public string NextImagePath
        {
            get
            {
                return GetImagePath("next.png");
            }
        }
    }
}
