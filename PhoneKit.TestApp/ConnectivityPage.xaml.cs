using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneKit.Framework.OS;
using PhoneKit.Framework.Core.Net;
using System.Windows.Media;
using System.Windows.Threading;

namespace PhoneKit.TestApp
{
    public partial class ConnectivityPage : PhoneApplicationPage
    {
        /// <summary>
        /// The neutral color brush.
        /// </summary>
        private readonly Brush NEUTRAL_BRUSH = new SolidColorBrush(Colors.White);

        /// <summary>
        /// The neutral color brush.
        /// </summary>
        private readonly Brush ACTIVE_BRUSH = new SolidColorBrush(Colors.Green);

        /// <summary>
        /// The neutral color brush.
        /// </summary>
        private readonly Brush INACTIVE_BRUSH = new SolidColorBrush(Colors.Red);

        /// <summary>
        /// Creates a ConnectivityPage instance.
        /// </summary>
        public ConnectivityPage()
        {
            InitializeComponent();

            GoToAirplaneSetting.Click += async (s, e) =>
                {
                    await SettingsLauncher.LaunchAirplaneModeAsync();
                };

            GoToWifiSetting.Click += async (s, e) =>
                {
                    await SettingsLauncher.LaunchWifiAsync();
                };

            GoToCellularSetting.Click += async (s, e) =>
                {
                    await SettingsLauncher.LaunchCellularAsync();
                };
        }

        /// <summary>
        /// When the page is navigated to.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ResetHighlighting();

            // direct status
            Network.Foreground = GetStatusBrush(ConnectivityHelper.HasNetwork);
            Wifi.Foreground = GetStatusBrush(ConnectivityHelper.HasWifi);
            IsAirplaneMode.Foreground = GetStatusBrush(ConnectivityHelper.IsAirplaneMode);

            // delayed status
            var timer = new DispatcherTimer();
            timer.Tick += (s, ev) =>
            {
                NetworkDelayed.Foreground = GetStatusBrush(ConnectivityHelper.HasNetwork);
                WifiDelayed.Foreground = GetStatusBrush(ConnectivityHelper.HasWifi);
                IsAirplaneModeDelayed.Foreground = GetStatusBrush(ConnectivityHelper.IsAirplaneMode);
            };
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }

        /// <summary>
        /// Gets the status brush color.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>Returns green for true and red for false.</returns>
        private Brush GetStatusBrush(bool status)
        {
            return status ? ACTIVE_BRUSH : INACTIVE_BRUSH;
        }

        /// <summary>
        /// Resets the color highlighting.
        /// </summary>
        private void ResetHighlighting()
        {
            Network.Foreground = NEUTRAL_BRUSH;
            Wifi.Foreground = NEUTRAL_BRUSH;
            IsAirplaneMode.Foreground = NEUTRAL_BRUSH;
            NetworkDelayed.Foreground = NEUTRAL_BRUSH;
            WifiDelayed.Foreground = NEUTRAL_BRUSH;
            IsAirplaneModeDelayed.Foreground = NEUTRAL_BRUSH;
        }
    }
}