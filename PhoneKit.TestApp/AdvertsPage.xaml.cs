using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

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
        }

        /// <summary>
        /// Handles the DoubleClick banner received event.
        /// </summary>
        private void DoubleClickAdControl_AdReceived(object sender, EventArgs e)
        {
            DoubleClickStatus.Text = "Received!";
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