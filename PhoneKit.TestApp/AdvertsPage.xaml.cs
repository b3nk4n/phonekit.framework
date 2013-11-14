using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneKit.Framework.Advertising;

namespace PhoneKit.TestApp
{
    /// <summary>
    /// The advertising test page.
    /// </summary>
    public partial class AdvertsPage : PhoneApplicationPage
    {
        /// <summary>
        /// Creates a AdvertPage instance.
        /// </summary>
        public AdvertsPage()
        {
            InitializeComponent();

            LoadDynamic();
        }

        /// <summary>
        /// Loads the StudiCluster Web-Banner
        /// </summary>
        private void LoadDynamic()
        {
            if (DynamicContainer.Children.Count <= 1)
            {
                DoubleClickAdControl adControl = new DoubleClickAdControl();
                adControl.Name = "WebBanner";
                // hide for smooth slide in
                adControl.Height = 0;

                adControl.BannerUri = new Uri("http://bsautermeister.de/phonekit/adverts/test_sc.html", UriKind.Absolute);
                adControl.AdReceived += (s, e) =>
                {
                    AdInTransition.Begin();
                    DoubleClickDynamicStatus.Text = "Received!";
                };
                adControl.Start();

                DynamicContainer.Children.Insert(0, adControl);
            }
        }

        /// <summary>
        /// Handles the DoubleClick banner received event.
        /// </summary>
        private void DoubleClickAdControl_AdReceived(object sender, EventArgs e)
        {
            DoubleClickStatus.Text = "Received!";
            StaticAdInTransition.Begin();
        }

        /// <summary>
        /// Handles the MsDuplex banner received event.
        /// </summary>
        private void MsDuplexAdControl_AdReceived(object sender, EventArgs e)
        {
            MsDuplexStatus.Text = "Received!";
        }
    }
}