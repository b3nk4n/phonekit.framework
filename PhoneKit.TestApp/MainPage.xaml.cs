﻿using System;
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

                var wideImage = GraphicsHelper.Create(new CustomWideControl());
                IList<Uri> wideImages = new List<Uri>();
                wideImages.Add(StorageHelper.SaveJpeg(LiveTileHelper.SHARED_SHELL_CONTENT_PATH + "test2.jpeg", wideImage));
                wideImages.Add(StorageHelper.SaveJpeg(LiveTileHelper.SHARED_SHELL_CONTENT_PATH + "test3.jpeg", wideImage));

                LiveTileHelper.PinOrUpdateTile(new Uri("/AboutPage.xaml", UriKind.Relative),
                    new CycleTileData
                    {
                        Title = "TEST TILE",
                        Count = 1,
                        SmallBackgroundImage = imageUri,
                        CycleImages = wideImages
                    }, true);
            };

            ApplicationBarIconButton appBarButton2 = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.settings.png", UriKind.Relative));
            appBarButton2.Text = "Settings";
            ApplicationBar.Buttons.Add(appBarButton2);
            appBarButton2.Click += (s, e) =>
            {
                NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
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

        private void Advertising_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AdvertsPage.xaml", UriKind.Relative));
        }

        private void Voice_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/VoicePage.xaml", UriKind.Relative));
        }

        private void Mvvm_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MvvmPage.xaml", UriKind.Relative));
        }
    }
}