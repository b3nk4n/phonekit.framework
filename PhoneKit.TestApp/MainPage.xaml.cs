﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneKit.TestApp.Resources;
using PhoneKit.Framework.Tile;
using PhoneKit.Framework.Core.LockScreen;
using PhoneKit.Framework.Core.Graphics;
using PhoneKit.TestApp.ImageControls;
using PhoneKit.Framework.Core.Storage;
using PhoneKit.Framework.Core.Tile;
using PhoneKit.Framework.Voice;
using PhoneKit.Framework.Support;

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

            Loaded += (s, e) =>
            {
                // register voice commands
                Speech.Instance.InstallCommandSets(new Uri("ms-appx:///voicecommands.xml", UriKind.Absolute));
            };

            // register startup actions
            StartupActionManager.Instance.Register(2, ActionExecutionRule.LessOrEquals, () =>
            {
                MessageBox.Show("Less or Equals 2.");
            });
            StartupActionManager.Instance.Register(2, ActionExecutionRule.MoreOrEquals, () =>
            {
                MessageBox.Show("More or Equals 2.");
            });

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();
        }

        /// <summary>
        /// When main page gets active, disables idle detection (to not interrupt the speech)
        /// and try to parse voce commands from query string.
        /// </summary>
        /// <param name="e">The navigation args.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            StartupActionManager.Instance.Fire();

            // disable idle detection
            PhoneApplicationService.Current.UserIdleDetectionMode =
                IdleDetectionMode.Disabled;

            try
            {
                String commandName = NavigationContext.QueryString["voiceCommandName"];

                if (!string.IsNullOrEmpty(commandName))
                    await Speech.Instance.Synthesizer.SpeakTextAsync("The voice command was: +" + commandName);

                // clear the QueryString or the page will retain the current value
                NavigationContext.QueryString.Clear();
            }
            catch (Exception)
            {
                // this code block is reached if the app is accessed in a way other than voice commands, therefore, do nothing
            }
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

                LiveTilePinningHelper.PinOrUpdateTile(new Uri("/AboutPage.xaml", UriKind.Relative),
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