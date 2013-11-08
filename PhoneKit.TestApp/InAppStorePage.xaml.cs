using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneKit.Framework.InAppPurchase;
using System.Collections.ObjectModel;
using Store = Windows.ApplicationModel.Store;
using Windows.ApplicationModel.Store;
using PhoneKit.TestApp.Resources;

namespace PhoneKit.TestApp
{
    /// <summary>
    /// Code behind of the in-app store page.
    /// </summary>
    public partial class InAppStorePage : PhoneApplicationPage
    {
        /// <summary>
        /// Creates an in InAppStorePage instance.
        /// </summary>
        public InAppStorePage()
        {
            InitializeComponent();
        }
    }
}