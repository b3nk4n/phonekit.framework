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
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.pin.png", UriKind.Relative));
            appBarButton.Text = "PinToStart";
            ApplicationBar.Buttons.Add(appBarButton);
            appBarButton.Click += (s, e) =>
            {
                LiveTileHelper.PinOrUpdateTile(new Uri("/AboutPage.xaml", UriKind.Relative),
                    new StandardTileData {
                        Title = "TEST TILE",
                        Count = 1,
                        BackTitle = "TEST BACK",
                        BackgroundImage = new Uri("http://bsautermeister.de/scribblehunter/images/branding/logo.png", UriKind.Absolute)
                    });
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
    }
}