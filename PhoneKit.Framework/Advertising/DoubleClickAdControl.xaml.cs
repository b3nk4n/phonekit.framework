using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Tasks;
using System.Windows.Input;
using PhoneKit.Framework.Advertising.Extensions;
using System.Diagnostics;

namespace PhoneKit.Framework.Advertising
{
    /// <summary>
    /// The user control for Google DoubleClick DFP advertisment.
    /// </summary>
    public partial class DoubleClickAdControl : UserControl
    {
        #region Members

        /// <summary>
        /// An event that clients can listen whenever the banner notifies
        /// that it was rendered in the embedded web browser.
        /// </summary>
        public event EventHandler AdReceived;

        /// <summary>
        /// The banners web URI as a dependency property.
        /// </summary>
        public static readonly DependencyProperty BannerUriProperty =
            DependencyProperty.Register("BannerUri",
                typeof(Uri), typeof(DoubleClickAdControl),
                new PropertyMetadata(null));

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a DoubleClickAdControl instance.
        /// </summary>
        public DoubleClickAdControl()
        {
            InitializeComponent();

            WebBanner.Loaded += new RoutedEventHandler(Browser_Loaded);
            WebBanner.Navigating += Browser_Navigating;
            WebBanner.ScriptNotify += _browser_ScriptNotify;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invokes that an advertisment received event.
        /// </summary>
        protected virtual void OnAdReceived(EventArgs e)
        {
            if (AdReceived != null)
                AdReceived(this, e);
        }

        #endregion

        #region Events

        /// <summary>
        /// Calls the add-banner-loaded callback, when the banner was rendered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _browser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            Debug.WriteLine("ScritpNotify: " + e.Value);

            if (e.Value.Equals("ad_loaded"))
            {
                OnAdReceived(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Cancels the navigation to any page except the defined banner page.
        /// </summary>
        private void Browser_Navigating(object sender, NavigatingEventArgs e)
        {
            if (e.Uri != BannerUri)
            {
                if (BannerUri == null)
                {
                    Debug.WriteLine("Banner URI is not specified.");
                    return;
                }

                e.Cancel = true;

                WebBrowserTask fsBrowser = new WebBrowserTask();
                fsBrowser.Uri = e.Uri;
                fsBrowser.Show();
            }
        }

        /// <summary>
        /// Sets up the internal browsers events to stop scrolling and zooming.
        /// </summary>
        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            var border = WebBanner.Descendants<Border>().Last() as Border;
            
            border.ManipulationDelta += Border_ManipulationDelta;
            border.ManipulationCompleted += Border_ManipulationCompleted;
        }

        /// <summary>
        /// Stops the zoom and scroll interaction of the browser.
        /// </summary>
        private void Border_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            // suppress zoom
            if (e.FinalVelocities.ExpansionVelocity.X != 0.0 ||
                e.FinalVelocities.ExpansionVelocity.Y != 0.0 ||
                e.IsInertial)
                e.Handled = true;
        }

        /// <summary>
        /// Stops the zoom and scroll interaction of the browser.
        /// </summary>
        private void Border_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            // suppress zoom
            if (e.DeltaManipulation.Scale.X != 0.0 ||
                e.DeltaManipulation.Scale.Y != 0.0)
                e.Handled = true;

            // optionally suppress scrolling
            if (e.DeltaManipulation.Translation.X != 0.0 ||
                e.DeltaManipulation.Translation.Y != 0.0)
                e.Handled = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the banners source URI.
        /// </summary>
        public Uri BannerUri
        {
            get
            {
                return (Uri)GetValue(BannerUriProperty);
            }
            set
            {
                SetValue(BannerUriProperty, value);

                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    WebBanner.Navigate(value);
                }
            }
        }

        #endregion
    }
}
