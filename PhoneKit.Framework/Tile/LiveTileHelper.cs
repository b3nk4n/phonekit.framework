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
            if (DownloadHelper.IsWebFile(tileData.BackBackgroundImage))
                tileData.BackBackgroundImage = await DownloadHelper.LoadFileAsync(
                    tileData.BackBackgroundImage,
                    DownloadLocation.IsolatedStorage);

            if (DownloadHelper.IsWebFile(tileData.BackgroundImage))
                tileData.BackgroundImage = await DownloadHelper.LoadFileAsync(
                    tileData.BackgroundImage,
                    DownloadLocation.IsolatedStorage);
        }

        /// <summary>
        /// Checks the remote images.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        /// <returns></returns>
        private static async Task CheckRemoteImagesAsync(CycleTileData tileData)
        {
            if (DownloadHelper.IsWebFile(tileData.SmallBackgroundImage))
                tileData.SmallBackgroundImage = await DownloadHelper.LoadFileAsync(
                    tileData.SmallBackgroundImage,
                    DownloadLocation.IsolatedStorage);

            IList<Uri> cycleImages = new List<Uri>(tileData.CycleImages);
            for (int i = 0; i < cycleImages.Count; ++i)
            {
                if (DownloadHelper.IsWebFile(cycleImages[i]))
                    cycleImages[i] = await DownloadHelper.LoadFileAsync(
                        cycleImages[i],
                        DownloadLocation.IsolatedStorage);
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
            if (DownloadHelper.IsWebFile(tileData.SmallBackgroundImage))
                tileData.SmallBackgroundImage = await DownloadHelper.LoadFileAsync(
                    tileData.SmallBackgroundImage,
                    DownloadLocation.IsolatedStorage);

            if (DownloadHelper.IsWebFile(tileData.BackBackgroundImage))
                tileData.BackBackgroundImage = await DownloadHelper.LoadFileAsync(
                    tileData.BackBackgroundImage,
                    DownloadLocation.IsolatedStorage);

            if (DownloadHelper.IsWebFile(tileData.BackgroundImage))
                tileData.BackgroundImage = await DownloadHelper.LoadFileAsync(
                    tileData.BackgroundImage,
                    DownloadLocation.IsolatedStorage);

            if (DownloadHelper.IsWebFile(tileData.WideBackgroundImage))
                tileData.WideBackgroundImage = await DownloadHelper.LoadFileAsync(
                    tileData.WideBackgroundImage,
                    DownloadLocation.IsolatedStorage);

            if (DownloadHelper.IsWebFile(tileData.WideBackBackgroundImage))
                tileData.WideBackBackgroundImage = await DownloadHelper.LoadFileAsync(
                    tileData.WideBackBackgroundImage,
                    DownloadLocation.IsolatedStorage);
        }

        #endregion
    }
}