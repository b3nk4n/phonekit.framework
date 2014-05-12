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
using PhoneKit.Framework.InAppPurchase;
using PhoneKit.Framework.Core.Collections;

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
            Loaded += (s, e) =>
                {
                    AdFreeStatus.Text = InAppPurchaseHelper.IsProductActive("ad_free") ? "ad_free purchased" : "ad_free NOT purchased";
                };

            LoadDynamic();
            InitOfflineAdControl();
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            OfflineAdControl.Start();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            OfflineAdControl.Stop();
        }

        /// <summary>
        /// Initializes the offline adverts control.
        /// </summary>
        private void InitOfflineAdControl()
        {
            List<AdvertData> advertsList = new List<AdvertData>();
            advertsList.Add(new AdvertData(new Uri("/Assets/Adverts/pocketBRAIN_adduplex.png", UriKind.Relative), AdvertData.ActionTypes.Website, "http://bsautermeister.de"));
            advertsList.Add(new AdvertData(new Uri("/Assets/Adverts/voiceTIMER_adduplex.png", UriKind.Relative), AdvertData.ActionTypes.StoreSearchTerm, "Benjamin Sautermeister"));

            advertsList.ShuffleList();
            foreach (var advert in advertsList)
            {
                OfflineAdControl.AddAdvert(advert);
            }
            OfflineAdControl.Start();
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