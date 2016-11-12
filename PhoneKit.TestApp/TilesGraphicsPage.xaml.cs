using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneKit.Framework.Core.Tile;
using PhoneKit.Framework.Core.Storage;
using PhoneKit.TestApp.ImageControls;
using PhoneKit.Framework.Core.Graphics;
using PhoneKit.Framework.Tile;
using System.Windows.Media;

namespace PhoneKit.TestApp
{
    public partial class TilesGraphicsPage : PhoneApplicationPage
    {
        public TilesGraphicsPage()
        {
            InitializeComponent();
        }

        private void RenderAndPinJpegImageButton_Click(object sender, RoutedEventArgs e)
        {
            //LiveTileHelper.ClearStorage();

            var image = GraphicsHelper.Create(new CusomTile(Colors.Red));
            Uri imageUri = StorageHelper.SaveJpeg(LiveTileHelper.SHARED_SHELL_CONTENT_PATH + "test.jpeg", image);

            var wideImage = GraphicsHelper.Create(new CustomWideControl(Colors.Green));
            IList<Uri> wideImages = new List<Uri>();
            wideImages.Add(StorageHelper.SaveJpeg(LiveTileHelper.SHARED_SHELL_CONTENT_PATH + "test2.jpeg", wideImage));
            wideImages.Add(StorageHelper.SaveJpeg(LiveTileHelper.SHARED_SHELL_CONTENT_PATH + "test3.jpeg", wideImage));

            LiveTilePinningHelper.PinOrUpdateTile(new Uri("/AboutPage.xaml", UriKind.Relative),
                new CycleTileData
                {
                    Title = "JPEG TILE",
                    Count = 1,
                    SmallBackgroundImage = imageUri,
                    CycleImages = wideImages
                }, true);
        }

        private void RenderAndPinPngImageButton_Click(object sender, RoutedEventArgs e)
        {
            //LiveTileHelper.ClearStorage();

            var image = GraphicsHelper.Create(new CusomTile(Colors.Transparent));
            Uri imageUri = StorageHelper.SavePng(LiveTileHelper.SHARED_SHELL_CONTENT_PATH + "test.png", image);

            var wideImage = GraphicsHelper.Create(new CustomWideControl(Colors.Transparent));
            IList<Uri> wideImages = new List<Uri>();
            wideImages.Add(StorageHelper.SavePng(LiveTileHelper.SHARED_SHELL_CONTENT_PATH + "test2.png", wideImage));
            wideImages.Add(StorageHelper.SavePng(LiveTileHelper.SHARED_SHELL_CONTENT_PATH + "test3.png", wideImage));

            LiveTilePinningHelper.PinOrUpdateTile(new Uri("/TilesGraphicsPage.xaml", UriKind.Relative),
                new CycleTileData
                {
                    Title = "PNG TILE",
                    Count = 1,
                    SmallBackgroundImage = imageUri,
                    CycleImages = wideImages
                }, true);
        }

        private void RenderAndPinPngStandardImageButton_Click(object sender, RoutedEventArgs e)
        {
            var image = GraphicsHelper.Create(new CusomTile(Colors.Transparent));
            Uri imageUri = StorageHelper.SavePng(LiveTileHelper.SHARED_SHELL_CONTENT_PATH + "std1.png", image);

            var backImage = GraphicsHelper.Create(new CusomTile(Colors.Red));
            Uri backImageUri = StorageHelper.SavePng(LiveTileHelper.SHARED_SHELL_CONTENT_PATH + "std2.png", backImage);

            LiveTilePinningHelper.PinOrUpdateTile(new Uri("/MainPage.xaml", UriKind.Relative),
                new StandardTileData
                {
                    Title = "PNG STD TILE",
                    BackgroundImage = imageUri,
                    BackBackgroundImage = backImageUri
                });
        }
    }
}