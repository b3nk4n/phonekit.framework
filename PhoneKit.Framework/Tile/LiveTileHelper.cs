using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Phone.Shell;
using PhoneKit.Framework.Net;


namespace PhoneKit.Framework.Tile
{
    /// <summary>
    /// Implementation of a Live Tile service.
    /// </summary>
    public static class LiveTileHelper
    {
        #region Public Methods

        public static void UpdateDefaultTile(StandardTileData tileData)
        {
            PinOrUpdateTile(new Uri("/", UriKind.Relative), tileData);
        }

        /// <summary>
        /// Checks whether an equivalent tile exists.
        /// </summary>
        /// <param name="navigationUri">The tiles navigation URI.</param>
        /// <returns>Returns true, if an equivalent live tile exists, else false.</returns>
        public static bool TileExists(Uri navigationUri)
        {
            return GetTile(navigationUri) != null;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Pins a tile to start page.
        /// </summary>
        /// <param name="navigationUri">The path to the page for the navigation when the pinned tile was clicked.</param>
        /// <param name="tileData">The tile data.</param>
        public static void PinOrUpdateTile(Uri navigationUri, StandardTileData tileData)
        {
            if (TileExists(navigationUri))
                UpdateTile(navigationUri, tileData);
            else
                CreateTile(navigationUri, tileData);
        }

        /// <summary>
        /// Creates a live tile.
        /// </summary>
        /// <param name="navigationUri">The navigation URI.</param>
        /// <param name="tileData">The tile data.</param>
        private static async void CreateTile(Uri navigationUri, StandardTileData tileData)
        {
            await CheckRemoteImagesAsync(tileData);

            ShellTile.Create(navigationUri, tileData);
        }

        /// <summary>
        /// Updates a live tile.
        /// </summary>
        /// <param name="navigationUri">The navigation URI.</param>
        /// <param name="tileData">The live tile data.</param>
        private static async void UpdateTile(Uri navigationUri, StandardTileData tileData)
        {
            await CheckRemoteImagesAsync(tileData);

            var activeTile = GetTile(navigationUri);
            
            if (activeTile != null)
                activeTile.Update(tileData);
        }

        /// <summary>
        /// Gets the tile which navigates to the given URI.
        /// </summary>
        /// <param name="navigationUri">The navigation URI.</param>
        /// <returns>Teturns true if the tile already exists, else false.</returns>
        private static ShellTile GetTile(Uri navigationUri)
        {
            foreach (var tile in ShellTile.ActiveTiles)
            {
                if (tile.NavigationUri.ToString() == navigationUri.ToString())
                    return tile;
            }

            return null;
        }

        /// <summary>
        /// Checks the remote images.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        /// <returns></returns>
        private static async Task CheckRemoteImagesAsync(StandardTileData tileData)
        {
            if (tileData.BackBackgroundImage != null)
                tileData.BackBackgroundImage = await GetTileImagePathAsync(tileData.BackBackgroundImage);
            if (tileData.BackgroundImage != null)
                tileData.BackgroundImage = await GetTileImagePathAsync(tileData.BackgroundImage);
        }

        /// <summary>
        /// Gets the live tiels image path.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        /// <param name="image">The image of the tile or if empty, the one of the life tile data is used.</param>
        /// <returns>The tiles image path, eighter from isolated storage or from resources.</returns>
        private static async Task<Uri> GetTileImagePathAsync(Uri imageUri)
        {
            try
            {
                // check if it is an image from web
                if (imageUri.OriginalString.StartsWith("http"))
                {
                    var localUri = "/shared/shellcontent/" + Path.GetFileName(imageUri.LocalPath);
                    return await DownloadHelper.LoadFileAsync(imageUri, localUri);                
                }

                // get image from resources
                return imageUri;
            }
            catch (Exception ex)
            {
				Debug.WriteLine("Getting the tile image path failed with error: " + ex.Message);
                return null;
            }
        }

        #endregion
    }
}