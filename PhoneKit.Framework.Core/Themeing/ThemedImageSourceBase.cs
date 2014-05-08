
namespace PhoneKit.Framework.Core.Themeing
{
    /// <summary>
    /// The base class for a themed image source.
    /// </summary>
    public abstract class ThemedImageSourceBase
    {
        /// <summary>
        /// The base path for themed images, e.g. "/Assets/Images"
        /// </summary>
        public string BasePath { get; private set; }

        /// <summary>
        /// Gets the dark folder name.
        /// </summary>
        public string DarkFolderName { get; private set; }

        /// <summary>
        /// Gets the light folder name.
        /// </summary>
        public string LightFolderName { get; private set; }

        /// <summary>
        /// Creates a ThemedImageSourceBase instance with default folder names "dark" und "light".
        /// </summary>
        /// <param name="basePath">The base path.</param>
        public ThemedImageSourceBase(string basePath)
            : this (basePath, "dark", "light")
        {
        }

        /// <summary>
        /// Creates a ThemedImageSourceBase instance.
        /// </summary>
        /// <param name="basePath">The base path.</param>
        /// <param name="darkFolderName">The dark folder name.</param>
        /// <param name="lightFolderName">The light folder name.</param>
        public ThemedImageSourceBase(string basePath, string darkFolderName, string lightFolderName)
        {
            BasePath = basePath;
            DarkFolderName = darkFolderName;
            LightFolderName = lightFolderName;
        }

        /// <summary>
        /// Gets the themed image path of the given resource.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The themed image path.</returns>
        protected string GetImagePath(string fileName)
        {
            string themeFolder;
            if (PhoneThemeHelper.IsDarkThemeActive)
                themeFolder = DarkFolderName;
            else
                themeFolder = LightFolderName;
            return string.Format("{0}/{1}/{2}", BasePath, themeFolder, fileName);
        }
    }
}
