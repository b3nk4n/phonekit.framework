using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;

namespace PhoneKit.Framework.Advertising
{
    /// <summary>
    /// An adverts control which uses MS Advertising SDK and AdDuplex as fallback.
    /// </summary>
    /// <remarks>
    /// Requires ID_CAP_IDENTITY_USER, ID_CAP_MEDIALIB_PHOTO and ID_CAP_PHONEDIALER!
    /// </remarks>
    public partial class MsDuplexAdControl : UserControl
    {
        /// <summary>
        /// An event that clients can listen whenever the banner notifies it was received.
        /// </summary>
        public event EventHandler AdReceived;

        /// <summary>
        /// The fallback ad units ID for AdDuplex.
        /// </summary>
        private string _adDuplexAppId;

        /// <summary>
        /// Indicates that the app runs in debug or test mode.
        /// </summary>
        private bool _isTest;

        /// <summary>
        /// Creates a FallbackAdControl instance.
        /// </summary>
        public MsDuplexAdControl()
        {
            InitializeComponent();

            MsBanner.ErrorOccurred += (s, e) =>
            {
                SwitchToFallback();
            };

            MsBanner.AdRefreshed += (s, e) =>
            {
                OnAdReceived(EventArgs.Empty);
            };
        }

        /// <summary>
        /// Invokes that an advertisment received event.
        /// </summary>
        protected virtual void OnAdReceived(EventArgs e)
        {
            if (AdReceived != null)
                AdReceived(this, e);
        }

        /// <summary>
        /// Switches to the fallback banner which used AdDuplex.
        /// </summary>
        private void SwitchToFallback()
        {
            // add adduplex control if AppId defined
            if (!string.IsNullOrEmpty(_adDuplexAppId))
            {
                // remove previous banner.
                LayoutRoot.Children.RemoveAt(0);

                // add new adduplex banner.
                try
                {
                    AdDuplex.AdControl adDuplex = new AdDuplex.AdControl();
                    adDuplex.Width = 480;
                    adDuplex.Height = 80;
                    adDuplex.AppId = _adDuplexAppId;
                    adDuplex.AdLoaded += (s, e) =>
                    {
                        OnAdReceived(EventArgs.Empty);
                    };

                    LayoutRoot.Children.Add(adDuplex);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Creation of AdDuplex banner failed with error: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Gets or sets the MS Advertising Applicatoin ID.
        /// </summary>
        public string MsApplicationId
        {
            get
            {
                return MsBanner.ApplicationId;
            }
            set
            {
                MsBanner.ApplicationId = value;
            }
        }

        /// <summary>
        /// Gets or sets the MS Advertising AdUnit ID.
        /// </summary>
        public string MsAdUnitId
        {
            get
            {
                return MsBanner.AdUnitId;
            }
            set
            {
                if (!_isTest)
                    MsBanner.AdUnitId = value;
            }
        }

        /// <summary>
        /// Gets or sets the AdDuplex App ID.
        /// </summary>
        public string AdDuplexAppId
        {
            get
            {
                return _adDuplexAppId;
            }
            set
            {
                if (!_isTest)
                    _adDuplexAppId = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the app runs in test or debug mode.
        /// </summary>
        public bool IsTest
        {
            get
            {
                return _isTest;
            }
            set
            {
                _isTest = value;

                if (_isTest == true)
                {
                    // set test values
                    MsBanner.ApplicationId = "test_client";
                    MsBanner.AdUnitId = "Image480_80";
                    _adDuplexAppId = "62359";
                }
            }
        }
    }
}
