using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneKit.TestApp.Resources;
using PhoneKit.Framework.Tile;
using PhoneKit.Framework.LockScreen;
using PhoneKit.Framework.Graphics;
using PhoneKit.TestApp.ImageControls;
using PhoneKit.Framework.Storage;

namespace PhoneKit.TestApp
{
    /// <summary>
    /// The main page.
    /// </summary>
    public partial class MainPage : PhoneApplicationPage
    {
        /// <summary>
        /// Creates a MainPage instance.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();
        }

        /// <summary>
        /// Builds a localized application bar.
        /// </summary>
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton1 = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.pin.png", UriKind.Relative));
            appBarButton1.Text = "Pin Custom";
            ApplicationBar.Buttons.Add(appBarButton1);
            appBarButton1.Click += (s, e) =>
            {
                LiveTileHelper.ClearStorage();

                var image = GraphicsHelper.Create(new CusomTile());
                Uri imageUri = StorageHelper.SaveJpeg(LiveTileHelper.SHARED_SHELL_CONTENT_PATH + "test.jpeg", image);

                LiveTileHelper.PinOrUpdateTile(new Uri("/AboutPage.xaml", UriKind.Relative),
                    new StandardTileData
                    {
                        Title = "TEST TILE",
                        Count = 1,
                        BackTitle = "TEST BACK",
                        BackgroundImage = new Uri("/Assets/ApplicationIcon.png", UriKind.Relative),
                        //new Uri("http://bsautermeister.de/scribblehunter/images/branding/logo.png", UriKind.Absolute),
                        BackBackgroundImage = imageUri
                    });
            };

            ApplicationBarIconButton appBarButton2 = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.pin.png", UriKind.Relative));
            appBarButton2.Text = "Pin Iconic";
            ApplicationBar.Buttons.Add(appBarButton2);
            appBarButton2.Click += (s, e) =>
            {
                LiveTileHelper.ClearStorage();

                LiveTileHelper.PinOrUpdateTile(new Uri("/InAppStorePage.xaml", UriKind.Relative),
                    new IconicTileData
                    {
                        Title = "TEST TILE",
                        Count = 1,
                        WideContent1 = "Stammessen",
                        WideContent2 = "Spätzle mit Soße",
                        WideContent3 = "mit Bratkartoffeln",
                        IconImage = new Uri("/Assets/ApplicationIcon.png", UriKind.Relative),
                        SmallIconImage = new Uri("/Assets/ApplicationIcon.png", UriKind.Relative),
                        BackgroundColor = System.Windows.Media.Color.FromArgb(255, 50, 50, 50)
                    },
                    true);
            };

            ApplicationBarIconButton appBarButton3 = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.lock.png", UriKind.Relative));
            appBarButton3.Text = "Lockscreen";
            ApplicationBar.Buttons.Add(appBarButton3);
            appBarButton3.Click += async (s, e) =>
            {
                if (await LockScreenHelper.VerifyAccessAsync())
                {
                    LockScreenHelper.ClearStorage();

                    LockScreenHelper.SetLockScreenImage(new Uri("http://imagelib.de/forum/colorscale_n.png", UriKind.Absolute));
                    //LockScreenHelper.SetLockScreenImage(new Uri("/Assets/MetroAlignmentGrid.png"));
                    //LockScreenHelper.SetLockScreenImage(new Uri("ms-appdata:///Assets/MetroAlignmentGrid.png"));
                }
            };

            // in-app sotre
            ApplicationBarMenuItem appBarMenuItem1 = new ApplicationBarMenuItem(AppResources.InAppStoreTitle);
            ApplicationBar.MenuItems.Add(appBarMenuItem1);
            appBarMenuItem1.Click += (s, e) =>
            {
                NavigationService.Navigate(new Uri("/InAppStorePage.xaml", UriKind.Relative));
            };

            // about
            ApplicationBarMenuItem appBarMenuItem2 = new ApplicationBarMenuItem(AppResources.AboutTitle);
            ApplicationBar.MenuItems.Add(appBarMenuItem2);
            appBarMenuItem2.Click += (s, e) =>
            {
                NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AdvertsPage.xaml", UriKind.Relative));
        }
    }
}