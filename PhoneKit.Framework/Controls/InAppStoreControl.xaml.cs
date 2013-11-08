using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Store;
using Store = Windows.ApplicationModel.Store;
using PhoneKit.Framework.InAppPurchase;

namespace PhoneKit.Framework.Controls
{
    /// <summary>
    /// The in-app store control
    /// </summary>
    public partial class InAppStoreControl : UserControl
    {
        /// <summary>
        /// In localized no products text as a dependency property.
        /// </summary>
        public static readonly DependencyProperty InAppStoreNoProductsTextProperty =
            DependencyProperty.Register("InAppStoreNoProductsText",
            typeof(string), typeof(InAppStoreControl),
            new PropertyMetadata("No in-app product available."));

        /// <summary>
        /// In localized buy button text as a dependency property.
        /// </summary>
        public static readonly DependencyProperty InAppStoreBuyTextProperty =
            DependencyProperty.Register("InAppStoreBuyText",
            typeof(string), typeof(InAppStoreControl),
            new PropertyMetadata("Buy"));

        /// <summary>
        /// In localized purchased text as a dependency property.
        /// </summary>
        public static readonly DependencyProperty InAppStorePurchasedTextProperty =
            DependencyProperty.Register("InAppStorePurchasedText",
            typeof(string), typeof(InAppStoreControl),
            new PropertyMetadata("Purchased"));

        /// <summary>
        /// The supported product IDs as a comma seperated string list
        /// </summary>
        private string _supportedProductIds = string.Empty;

        /// <summary>
        /// Creates an InAppStoreControl instance.
        /// </summary>
        public InAppStoreControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Updates the products of the in-app store.
        /// </summary>
        public async void UpdateProducts()
        {
            // verify supported product configuration
            if (string.IsNullOrEmpty(_supportedProductIds))
                throw new InvalidOperationException("There are no supported products.");

            // load products
            IList<ProductItem> products = await InAppPurchaseHelper.LoadProductsAsync(_supportedProductIds.Split(',').ToList(),
                InAppStorePurchasedText);

            if (products.Count > 0)
            {
                NoProductsInfo.Visibility = Visibility.Collapsed;

                // display loaded products
                ProductItemsList.ItemsSource = products;
            }
            else
            {
                NoProductsInfo.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Click event handler for each BUY button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private async void BuyButton_Clicked(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            // get the associated product ID
            string productId = btn.Tag.ToString();

            // purchase product if possible
            if (!InAppPurchaseHelper.IsProductActive(productId))
            {
                await InAppPurchaseHelper.RequestProductPurchaseAsync(productId);

                UpdateProducts();
            }
        }

        /// <summary>
        /// Gets or sets the localized purchased text.
        /// </summary>
        public string InAppStorePurchasedText
        {
            get { return (string)GetValue(InAppStorePurchasedTextProperty); }
            set { SetValue(InAppStorePurchasedTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the localized no products text.
        /// </summary>
        public string InAppStoreNoProductsText
        {
            get { return (string)GetValue(InAppStoreNoProductsTextProperty); }
            set { SetValue(InAppStoreNoProductsTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the localized buy button text.
        /// </summary>
        public string InAppStoreBuyText
        {
            get { return (string)GetValue(InAppStoreBuyTextProperty); }
            set { SetValue(InAppStoreBuyTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the supported product IDs as a comma seperated string.
        /// </summary>
        public string SupportedProductIds
        {
            get
            {
                return _supportedProductIds;
            }
            set
            {
                _supportedProductIds = value;
            }
        }
    }
}
