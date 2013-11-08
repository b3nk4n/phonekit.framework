namespace PhoneKit.Framework.Tile
{
    /// <summary>
    /// Represents the data of a live tile.
    /// </summary>
    public class TileData
    {
        /// <summary>
        /// Gets or sets the identifier of the current notification showed in the tile.
        /// </summary>
        public string CurrentId { get; set; }

        /// <summary>
        /// Gets or sets the title of the tile of the tile.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the title showed in background of the tile.
        /// </summary>
        public string BackTitle { get; set; }

        /// <summary>
        /// Gets or sets the counter for tile notifications.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets sets the content of the back side of the tile.
        /// </summary>
        public string BackContent { get; set; }

        /// <summary>
        /// Gets or sets the background image of the tile.
        /// </summary>
        public string BackgroundImagePath { get; set; }

        /// <summary>
        /// Gets or sets the background image of the back side of the tile.
        /// </summary>
        public string BackBackgroundImagePath { get; set; }

        /// <summary>
        /// Gets or sets the logo of the tile.
        /// </summary>
        public string LogoPath { get; set; }

        /// <summary>
        /// Gets sets the url of the notification showed in the tile.
        /// </summary>
        public string Url { get; set; }
    }
}