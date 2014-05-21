using System;
using System.Threading.Tasks;
using Microsoft.Phone.Shell;
using System.Collections.Generic;
using PhoneKit.Framework.Core.Net;
using System.Diagnostics;


namespace PhoneKit.Framework.Core.Tile
{
    /// <summary>
    /// Implementation of a Live Tile helper class for live tile updating.
    /// </summary>
    /// <remarks>
    /// These helper methods have been extracted form the LiveTileHelper, because
    /// a background task is just allowed to reference a subset of its functions
    /// </remarks>
    public static class LiveTileHelper
    {
        #region Members

        /// <summary>
        /// The base path of the shared shell content, which is the root for all live tile images.
        /// </summary>
        public const string SHARED_SHELL_CONTENT_PATH = "/shared/shellcontent/";

        /// <summary>
        /// The download manager.
        /// </summary>
        private static readonly DownloadManager _downloadManager = new DownloadManager(
            SHARED_SHELL_CONTENT_PATH,
            DownloadStorageLocation.IsolatedStorage);

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the default live tile.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        public static void UpdateDefaultTile(StandardTileData tileData)
        {
            UpdateTileInternal(new Uri("/", UriKind.Relative), tileData);
        }

        /// <summary>
        /// Updates the default live tile.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        public static void UpdateDefaultTile(IconicTileData tileData)
        {
            UpdateTileInternal(new Uri("/", UriKind.Relative), tileData);
        }

        /// <summary>
        /// Updates the default live tile.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        public static void UpdateDefaultTile(CycleTileData tileData)
        {
            UpdateTileInternal(new Uri("/", UriKind.Relative), tileData);
        }

        /// <summary>
        /// Updates the default live tile.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        public static void UpdateDefaultTile(FlipTileData tileData)
        {
            UpdateTileInternal(new Uri("/", UriKind.Relative), tileData);
        }

        /// <summary>
        /// Updates a tile.
        /// </summary>
        /// <param name="navigationUri">The path to the page for the navigation when the pinned tile was clicked.</param>
        /// <param name="tileData">The tile data.</param>
        public static async Task<bool> UpdateTile(Uri navigationUri, StandardTileData tileData)
        {
            if (TileExists(navigationUri))
            {
                await CheckRemoteImagesAsync(tileData);
                UpdateTileInternal(navigationUri, tileData);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates a tile.
        /// </summary>
        /// <param name="navigationUri">The path to the page for the navigation when the pinned tile was clicked.</param>
        /// <param name="tileData">The tile data.</param>
        /// <param name="supportsWideTile">Whether the tile supports the wide mode.</param>
        public static bool UpdateTile(Uri navigationUri, IconicTileData tileData, bool supportsWideTile)
        {
            // note: no web image check required, because not supported for iconic tile
            if (TileExists(navigationUri))
            {
                UpdateTileInternal(navigationUri, tileData);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates a tile.
        /// </summary>
        /// <param name="navigationUri">The path to the page for the navigation when the pinned tile was clicked.</param>
        /// <param name="tileData">The tile data.</param>
        /// <param name="supportsWideTile">Whether the tile supports the wide mode.</param>
        public static async Task<bool> UpdateTile(Uri navigationUri, CycleTileData tileData, bool supportsWideTile)
        {
            if (TileExists(navigationUri))
            {
                await CheckRemoteImagesAsync(tileData);
                UpdateTileInternal(navigationUri, tileData);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates a tile.
        /// </summary>
        /// <param name="navigationUri">The path to the page for the navigation when the pinned tile was clicked.</param>
        /// <param name="tileData">The tile data.</param>
        /// <param name="supportsWideTile">Whether the tile supports the wide mode.</param>
        public static async Task<bool> UpdateTile(Uri navigationUri, FlipTileData tileData, bool supportsWideTile)
        {
            if (TileExists(navigationUri))
            {
                await CheckRemoteImagesAsync(tileData);
                UpdateTileInternal(navigationUri, tileData);
                return true;
            }
            return false;
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

        /// <summary>
        /// Clears the used storage of downloaded files.
        /// </summary>
        public static void ClearStorage()
        {
            _downloadManager.Clear();
        }

        /// <summary>
        /// Removes a pinned secondary tile in case it exists.
        /// </summary>
        /// <param name="navigationUri">The unique navigation URI of the pinned tile.</param>
        public static void RemoveTile(Uri navigationUri)
        {
            var activeTile = GetTile(navigationUri);

            if (activeTile != null)
            {
                try
                {
                    activeTile.Delete();
                }
                catch (InvalidOperationException ioe)
                {
                    // BUGSENSE: could be related to the following tile issue
                    // [15 May 2014 14:50; 1 time] Tiles can only be created when the application is in the foreground
                    Debug.WriteLine("Deleting tile failed: " + ioe.Message);
                }
            }

        }

        /// <summary>
        /// Checks the remote images.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        /// <returns></returns>
        public static async Task CheckRemoteImagesAsync(StandardTileData tileData)
        {
            if (_downloadManager.IsWebFile(tileData.BackBackgroundImage))
                tileData.BackBackgroundImage = await _downloadManager.LoadFileAsync(
                    tileData.BackBackgroundImage);

            if (_downloadManager.IsWebFile(tileData.BackgroundImage))
                tileData.BackgroundImage = await _downloadManager.LoadFileAsync(
                    tileData.BackgroundImage);
        }

        /// <summary>
        /// Checks the remote images.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        /// <returns></returns>
        public static async Task CheckRemoteImagesAsync(CycleTileData tileData)
        {
            if (_downloadManager.IsWebFile(tileData.SmallBackgroundImage))
                tileData.SmallBackgroundImage = await _downloadManager.LoadFileAsync(
                    tileData.SmallBackgroundImage);

            IList<Uri> cycleImages = new List<Uri>(tileData.CycleImages);
            for (int i = 0; i < cycleImages.Count; ++i)
            {
                if (_downloadManager.IsWebFile(cycleImages[i]))
                    cycleImages[i] = await _downloadManager.LoadFileAsync(
                        cycleImages[i]);
            }
            tileData.CycleImages = cycleImages;
        }

        /// <summary>
        /// Checks the remote images.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        /// <returns></returns>
        public static async Task CheckRemoteImagesAsync(FlipTileData tileData)
        {
            if (_downloadManager.IsWebFile(tileData.SmallBackgroundImage))
                tileData.SmallBackgroundImage = await _downloadManager.LoadFileAsync(
                    tileData.SmallBackgroundImage);

            if (_downloadManager.IsWebFile(tileData.BackBackgroundImage))
                tileData.BackBackgroundImage = await _downloadManager.LoadFileAsync(
                    tileData.BackBackgroundImage);

            if (_downloadManager.IsWebFile(tileData.BackgroundImage))
                tileData.BackgroundImage = await _downloadManager.LoadFileAsync(
                    tileData.BackgroundImage);

            if (_downloadManager.IsWebFile(tileData.WideBackgroundImage))
                tileData.WideBackgroundImage = await _downloadManager.LoadFileAsync(
                    tileData.WideBackgroundImage);

            if (_downloadManager.IsWebFile(tileData.WideBackBackgroundImage))
                tileData.WideBackBackgroundImage = await _downloadManager.LoadFileAsync(
                    tileData.WideBackBackgroundImage);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates a live tile.
        /// </summary>
        /// <param name="navigationUri">The navigation URI.</param>
        /// <param name="tileData">The live tile data.</param>
        private static void UpdateTileInternal(Uri navigationUri, ShellTileData tileData)
        {
            var activeTile = GetTile(navigationUri);
            
            if (activeTile != null)
            {
                try
                {
                    activeTile.Update(tileData);
                }
                catch (InvalidOperationException ioe)
                {
                    // BUGSENSE: could be related to the following tile issue
                    // [15 May 2014 14:50; 1 time] Tiles can only be created when the application is in the foreground
                    Debug.WriteLine("Deleting tile failed: " + ioe.Message);
                }
            }
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

        #endregion
    }
}