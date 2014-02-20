using System;
using System.Threading.Tasks;
using Microsoft.Phone.Shell;
using System.Collections.Generic;
using PhoneKit.Framework.Core.Net;
using PhoneKit.Framework.Core.Tile;


namespace PhoneKit.Framework.Tile
{
    /// <summary>
    /// Implementation of a Live Tile service.
    /// </summary>
    public static class LiveTilePinningHelper
    {
        #region Public Methods

        /// <summary>
        /// Pins a tile to start page.
        /// </summary>
        /// <param name="navigationUri">The path to the page for the navigation when the pinned tile was clicked.</param>
        /// <param name="tileData">The tile data.</param>
        public static async void PinOrUpdateTile(Uri navigationUri, StandardTileData tileData)
        {
            if (!await LiveTileHelper.UpdateTile(navigationUri, tileData))
            {
                await LiveTileHelper.CheckRemoteImagesAsync(tileData);
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
            if (!LiveTileHelper.UpdateTile(navigationUri, tileData, supportsWideTile))
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
            if (!await LiveTileHelper.UpdateTile(navigationUri, tileData, supportsWideTile))
            {
                await LiveTileHelper.CheckRemoteImagesAsync(tileData);
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
            if (!await LiveTileHelper.UpdateTile(navigationUri, tileData, supportsWideTile))
            {
                await LiveTileHelper.CheckRemoteImagesAsync(tileData);
                CreateTile(navigationUri, tileData, supportsWideTile);
            }
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

        #endregion
    }
}