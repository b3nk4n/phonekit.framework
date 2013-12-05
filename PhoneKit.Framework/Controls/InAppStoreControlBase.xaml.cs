using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PhoneKit.Framework.InAppPurchase;

namespace PhoneKit.Framework.Controls
{
    /// <summary>
    /// The in-app store control
    /// </summary>
    public abstract partial class InAppStoreControlBase : UserControl
    {
        #region Members

        /// <summary>
        /// The localized loading text.
        /// </summary>
        private string _inAppStoreLoadingText = "Loading...";

        /// <summary>
        /// The localized no products text.
        /// </summary>
        private string _inAppStoreNoProductsText = "No in-app product available.";

        /// <summary>
        /// The localized purchased text as a dependency property.
        /// </summary
        private string _inAppStorePurchasedText = "Purchased";

        /// <summary>
        /// The list of loaded products.
        /// </summary>
        private IList<ProductItem> _loadedProducts = new List<ProductItem>();

        /// <summary>
        /// The supported product IDs as a comma seperated string list
        /// </summary>
        private string _supportedProductIds = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an InAppStoreControl instance.
        /// </summary>
        public InAppStoreControlBase()
        {
            InitializeComponent();
            LocalizeContent();
            
            ShowMessage(_inAppStoreLoadingText);

            // show loading message und update products when page has loaded
            Loaded += (s, e) => {
                UpdateProducts();
            };

            ProductItemsList.SelectionChanged += ProductItemsList_SelectionChanged;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Localizes the user control content and texts.
        /// </summary>
        protected abstract void LocalizeContent();

        /// <summary>
        /// Updates the products of the in-app store.
        /// </summary>
        private async void UpdateProducts()
        {
            // verify supported product configuration
            if (string.IsNullOrEmpty(_supportedProductIds))
                throw new InvalidOperationException("There are no supported products.");

            _loadedProducts.Clear();

            // load products
            _loadedProducts = await InAppPurchaseHelper.LoadProductsAsync(_supportedProductIds.Split(',').ToList(),
                _inAppStorePurchasedText);

            if (_loadedProducts.Count > 0)
            {
                HideMessage();

                // display loaded products
                ProductItemsList.ItemsSource = _loadedProducts;
            }
            else
            {
                ShowMessage(_inAppStoreNoProductsText);
            }
        }

        /// <summary>
        /// Displays an info message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void ShowMessage(string message)
        {
            MessageInfo.Visibility = Visibility.Visible;
            MessageInfo.Text = message;
        }

        /// <summary>
        /// Hides the info message.
        /// </summary>
        private void HideMessage()
        {
            MessageInfo.Visibility = Visibility.Collapsed;
            MessageInfo.Text = string.Empty;
        }

        #endregion

        #region Events

        /// <summary>
        /// Called when a new products was selected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private async void ProductItemsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (ProductItemsList.SelectedIndex < 0)
                return;

            // get the associated product ID
            string productId = _loadedProducts[ProductItemsList.SelectedIndex].Id;

            // purchase product if possible
            if (!InAppPurchaseHelper.IsProductActive(productId))
            {
                await InAppPurchaseHelper.RequestProductPurchaseAsync(productId);

                UpdateProducts();
            }

            // Reset selected index to -1 (no selection)
            ProductItemsList.SelectedIndex = -1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the localized loading text.
        /// </summary>
        public string InAppStoreLoadingText
        {
            set
            {
                _inAppStoreLoadingText = value;
            }
        }

        /// <summary>
        /// Gets or sets the localized purchased text.
        /// </summary>
        public string InAppStorePurchasedText
        {
            set
            {
                _inAppStorePurchasedText = value;
            }
        }

        /// <summary>
        /// Gets or sets the localized no products text.
        /// </summary>
        public string InAppStoreNoProductsText
        {
            set
            {
                _inAppStoreNoProductsText = value;
            }
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

        #endregion
    }
}
