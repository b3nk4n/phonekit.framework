using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Phone.Shell;
using PhoneKit.Framework.Net;
using System.Collections.Generic;


namespace PhoneKit.Framework.Tile
{
    /// <summary>
    /// Implementation of a Live Tile service.
    /// </summary>
    public static class LiveTileHelper
    {
        #region Public Methods

        /// <summary>
        /// Updates the default live tile.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        public static void UpdateDefaultTile(StandardTileData tileData)
        {
            UpdateTile(new Uri("/", UriKind.Relative), tileData);
        }

        /// <summary>
        /// Updates the default live tile.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        public static void UpdateDefaultTile(IconicTileData tileData)
        {
            UpdateTile(new Uri("/", UriKind.Relative), tileData);
        }

        /// <summary>
        /// Updates the default live tile.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        public static void UpdateDefaultTile(CycleTileData tileData)
        {
            UpdateTile(new Uri("/", UriKind.Relative), tileData);
        }

        /// <summary>
        /// Updates the default live tile.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        public static void UpdateDefaultTile(FlipTileData tileData)
        {
            UpdateTile(new Uri("/", UriKind.Relative), tileData);
        }

        /// <summary>
        /// Pins a tile to start page.
        /// </summary>
        /// <param name="navigationUri">The path to the page for the navigation when the pinned tile was clicked.</param>
        /// <param name="tileData">The tile data.</param>
        public static async void PinOrUpdateTile(Uri navigationUri, StandardTileData tileData)
        {
            if (TileExists(navigationUri))
            {
                await CheckRemoteImagesAsync(tileData);
                UpdateTile(navigationUri, tileData);
            }
            else
            {
                await CheckRemoteImagesAsync(tileData);
                CreateTile(navigationUri, tileData, false);
            }
        }

        /// <summary>
        /// Pins a tile to start page.
        /// </summary>
        /// <param name="navigationUri">The path to the page for the navigation when the pinned tile was clicked.</param>
        /// <param name="tileData">The tile data.</param>
        /// <param name="supportsWideTile">Whether the tile supports the wide mode.</param>
        public static void PinOrUpdateTile(Uri navigationUri, IconicTileData tileData, bool supportsWideTile)
        {
            // note: no web image check required, because not supported for iconic tile
            if (TileExists(navigationUri))
            {
                UpdateTile(navigationUri, tileData);
            }
            else
            {
                CreateTile(navigationUri, tileData, supportsWideTile);
            }
        }

        /// <summary>
        /// Pins a tile to start page.
        /// </summary>
        /// <param name="navigationUri">The path to the page for the navigation when the pinned tile was clicked.</param>
        /// <param name="tileData">The tile data.</param>
        /// <param name="supportsWideTile">Whether the tile supports the wide mode.</param>
        public static async void PinOrUpdateTile(Uri navigationUri, CycleTileData tileData, bool supportsWideTile)
        {
            if (TileExists(navigationUri))
            {
                await CheckRemoteImagesAsync(tileData);
                UpdateTile(navigationUri, tileData);
            }
            else
            {
                await CheckRemoteImagesAsync(tileData);
                CreateTile(navigationUri, tileData, supportsWideTile);
            }
        }

        /// <summary>
        /// Pins a tile to start page.
        /// </summary>
        /// <param name="navigationUri">The path to the page for the navigation when the pinned tile was clicked.</param>
        /// <param name="tileData">The tile data.</param>
        /// <param name="supportsWideTile">Whether the tile supports the wide mode.</param>
        public static async void PinOrUpdateTile(Uri navigationUri, FlipTileData tileData, bool supportsWideTile)
        {
            if (TileExists(navigationUri))
            {
                await CheckRemoteImagesAsync(tileData);
                UpdateTile(navigationUri, tileData);
            }
            else
            {
                await CheckRemoteImagesAsync(tileData);
                CreateTile(navigationUri, tileData, supportsWideTile);
            }
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
        /// Creates a live tile.
        /// </summary>
        /// <param name="navigationUri">The navigation URI.</param>
        /// <param name="tileData">The tile data.</param>
        /// <param name="supportsWideTile">Whether the tile supports the wide mode.</param>
        private static void CreateTile(Uri navigationUri, ShellTileData tileData, bool supportsWideTile)
        {
            ShellTile.Create(navigationUri, tileData, supportsWideTile);
        }

        /// <summary>
        /// Updates a live tile.
        /// </summary>
        /// <param name="navigationUri">The navigation URI.</param>
        /// <param name="tileData">The live tile data.</param>
        private static void UpdateTile(Uri navigationUri, ShellTileData tileData)
        {
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
        /// Checks the remote images.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        /// <returns></returns>
        private static async Task CheckRemoteImagesAsync(CycleTileData tileData)
        {
            if (tileData.SmallBackgroundImage != null)
                tileData.SmallBackgroundImage = await GetTileImagePathAsync(tileData.SmallBackgroundImage);

            IList<Uri> cycleImages = new List<Uri>(tileData.CycleImages);
            for (int i = 0; i < cycleImages.Count; ++i)
            {
                if (cycleImages[i] != null)
                    cycleImages[i] = await GetTileImagePathAsync(cycleImages[i]);
            }
            tileData.CycleImages = cycleImages;
        }

        /// <summary>
        /// Checks the remote images.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        /// <returns></returns>
        private static async Task CheckRemoteImagesAsync(FlipTileData tileData)
        {
            if (tileData.SmallBackgroundImage != null)
                tileData.SmallBackgroundImage = await GetTileImagePathAsync(tileData.SmallBackgroundImage);

            if (tileData.BackBackgroundImage != null)
                tileData.BackBackgroundImage = await GetTileImagePathAsync(tileData.BackBackgroundImage);

            if (tileData.BackgroundImage != null)
                tileData.BackgroundImage = await GetTileImagePathAsync(tileData.BackgroundImage);

            if (tileData.WideBackgroundImage != null)
                tileData.WideBackgroundImage = await GetTileImagePathAsync(tileData.WideBackgroundImage);

            if (tileData.WideBackBackgroundImage != null)
                tileData.WideBackBackgroundImage = await GetTileImagePathAsync(tileData.WideBackBackgroundImage);
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
                    var localUri = "/shared/shellcontent/" + imageUri.LocalPath.Replace('/','_').Replace('\\', '_');
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