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
    public partial class AdvertsPage : PhoneApplicationPage
    {
        public AdvertsPage()
        {
            InitializeComponent();
        }

        private void DoubleClickAdControl_AdReceived(object sender, EventArgs e)
        {
            DoubleClickStatus.Text = "Received!";
        }
    }
}