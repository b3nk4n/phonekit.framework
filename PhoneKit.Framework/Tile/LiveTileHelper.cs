using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Phone.Shell;


namespace PhoneKit.Framework.Tile
{
    /// <summary>
    /// Implementation of a Live Tile service.
    /// </summary>
    public static class LiveTileHelper
    {
        /// <summary>
        /// The folder for downloaded content of the live tile in isolated storage.
        /// </summary>
        private const string LIVE_TILE_FOLDER = "/shared/shellcontent/";

        /// <summary>
        /// The base path of all resource images of the live tile
        /// </summary>
        private const string RESOURCE_PATH = "Assets/"; // TODO: check if this base path is really neccessary

        /// <summary>
        /// Pins a tile to start page.
        /// </summary>
        /// <param name="navigationUri">The path to the page for the navigation when the pinned tile was clicked.</param>
        /// <param name="tileData">The tile data.</param>
        public static void PinToStart(string navigationUri, TileData tileData)
        {
            navigationUri += string.Format("?currentID={0}", HttpUtility.UrlEncode(tileData.CurrentId) ?? string.Empty);
            
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
        private static async void CreateTile(string navigationUri, TileData tileData)
        {
            await CheckRemoteImages(tileData);
            var tile = GetStandardTileData(tileData);

            ShellTile.Create(new Uri(navigationUri, UriKind.Relative), tile);
        }

        /// <summary>
        /// Updates a live tile.
        /// </summary>
        /// <param name="navigationUri">The navigation URI.</param>
        /// <param name="tileData">The live tile data.</param>
        private static async void UpdateTile(string navigationUri, TileData tileData)
        {
            await CheckRemoteImages(tileData);
            var tile = GetStandardTileData(tileData);

            var activeTile = ShellTile.ActiveTiles.FirstOrDefault(t => t.NavigationUri.ToString().Contains(navigationUri));
            
            if (activeTile != null)
                activeTile.Update(tile);
        }

        /// <summary>
        /// Gets the standard tile data.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        /// <returns>Returns the standard tile data.</returns>
        private static StandardTileData GetStandardTileData(TileData tileData)
        {
            var tile = new StandardTileData
            {
                Title = tileData.Title ?? string.Empty,
                Count = tileData.Count,
                BackTitle = tileData.BackTitle ?? string.Empty,
                BackContent = tileData.BackContent ?? string.Empty
            };
            
            if (!string.IsNullOrEmpty(tileData.BackgroundImagePath))
                tile.BackgroundImage = new Uri(tileData.BackgroundImagePath, UriKind.RelativeOrAbsolute);
            
            if (!string.IsNullOrEmpty(tileData.BackgroundImagePath))
                tile.BackBackgroundImage = new Uri(tileData.BackBackgroundImagePath, UriKind.RelativeOrAbsolute);
            
            return tile;
        }

        /// <summary>
        /// Checks whether an equivalent tile exists.
        /// </summary>
        /// <param name="navigationUri">The tiles navigation URI.</param>
        /// <returns>Returns true, if an equivalent live tile exists, else false.</returns>
        private static bool TileExists(string navigationUri)
        {
            var tile = ShellTile.ActiveTiles.FirstOrDefault(o => o.NavigationUri.ToString().Contains(navigationUri));
            return tile != null;
        }

        /// <summary>
        /// Checks the remote images.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        /// <returns></returns>
        private static async Task CheckRemoteImages(TileData tileData)
        {
            if (!string.IsNullOrEmpty(tileData.BackBackgroundImagePath))
                tileData.BackBackgroundImagePath = await GetTileImagePath(tileData, tileData.BackBackgroundImagePath);
            if (!string.IsNullOrEmpty(tileData.BackgroundImagePath))
                tileData.BackgroundImagePath = await GetTileImagePath(tileData, tileData.BackgroundImagePath);
        }

        /// <summary>
        /// Gets the live tiels image path.
        /// </summary>
        /// <param name="tileData">The live tile data.</param>
        /// <param name="image">The image of the tile or if empty, the one of the life tile data is used.</param>
        /// <returns>The tiles image path, eighter from isolated storage or from resources.</returns>
        private static async Task<string> GetTileImagePath(TileData tileData, string image)
        {
            try
            {
                var transferUri = new Uri(Uri.EscapeUriString(image), UriKind.RelativeOrAbsolute);

                // check if it is an image from web
                if (transferUri.ToString().StartsWith("http"))
                {
                    var localUri = new Uri(LIVE_TILE_FOLDER + Path.GetFileName(transferUri.LocalPath),
                        UriKind.RelativeOrAbsolute);
                    await DownloadFile(transferUri, localUri);
                    return "isostore:" + localUri;                    
                }

                // get image from resources
                return RESOURCE_PATH +
                       Path.GetFileName(!string.IsNullOrEmpty(image) ? image : tileData.LogoPath);
            }
            catch (Exception ex)
            {
				Debug.WriteLine("Getting the tile image path failed with error: " + ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// Downloads a file from the web and stores it in isolated storage.
        /// </summary>
        /// <param name="webUri">The web URI.</param>
        /// <param name="localUri">The local URI in isolated storage.</param>
        /// <returns>The async task.</returns>
        private static async Task DownloadFile(Uri webUri, Uri localUri)
        {
            var request = WebRequest.CreateHttp(webUri);
            var task = Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse,
                request.EndGetResponse,
                null);
            await task.ContinueWith(t =>
            {
                using (var responseStream = task.Result.GetResponseStream())
                {
                    using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        try
                        {
                            using (var isoStoreFile = isoStore.OpenFile(localUri.ToString(),
                                FileMode.Create,
                                FileAccess.ReadWrite))
                            {
                                // store loaded data in isolated storage
                                var dataBuffer = new byte[1024];
                                while (responseStream.Read(dataBuffer, 0, dataBuffer.Length) > 0)
                                {
                                    isoStoreFile.Write(dataBuffer, 0, dataBuffer.Length);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Download file with tile data failed with error: " + ex.Message);
                        }
                    }
                }
            });
        }
    }
}