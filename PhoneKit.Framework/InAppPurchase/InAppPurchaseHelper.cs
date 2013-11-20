using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Store = Windows.ApplicationModel.Store;

namespace PhoneKit.Framework.InAppPurchase
{
    public static class InAppPurchaseHelper
    {
        /// <summary>
        /// Verifies whether the product is active/purchased.
        /// </summary>
        /// <param name="productId">The product ID.</param>
        /// <returns>
        /// Returns true if the product is active and has been purchased, else false.
        /// </returns>
        public static bool IsProductActive(string productId)
        {
            return Store.CurrentApp.LicenseInformation.ProductLicenses[productId].IsActive;
        }

        /// <summary>
        /// Request a product purchase.
        /// </summary>
        /// <param name="productId">The product ID to purchase.</param>
        /// <returns>
        /// Returns true if the purchase was successful, and false
        /// if the purchase was unsuccessful or the product has already been purchased.
        /// </returns>
        public static async Task<bool> RequestProductPurchaseAsync(string productId)
        {
            if (IsProductActive(productId))
                return false;
            try
            {
                await Store.CurrentApp.RequestProductPurchaseAsync(productId, false);
                return true;
            }
            catch (Exception)
            {
                // thrown when the user cancels the pruchase...
                return false;
            }
        }

        /// <summary>
        /// Loads the products asynchronously.
        /// </summary>
        /// <param name="supportedProductIds">The supporeted in-app product IDs.</param>
        /// <param name="localizedPurchasedText">The localized purchased text.</param>
        /// <returns></returns>
        public static async Task<IList<ProductItem>> LoadProductsAsync(IEnumerable<string> supportedProductIds, string localizedPurchasedText)
        {
            IList<ProductItem> productItems = new List<ProductItem>();
            
            try
            {
                // load supported products
                ListingInformation lisitingInfo = await Store.CurrentApp.LoadListingInformationByProductIdsAsync(supportedProductIds);

                foreach (string id in lisitingInfo.ProductListings.Keys)
                {
                    ProductListing product = lisitingInfo.ProductListings[id];
                    string status = Store.CurrentApp.LicenseInformation.ProductLicenses[id].IsActive ? localizedPurchasedText : product.FormattedPrice;

                    string imageLink = string.Empty;
                    productItems.Add(
                        new ProductItem
                        {
                            ImageUri = product.ImageUri,
                            Name = product.Name,
                            Description = product.Description,
                            Status = status,
                            Id = id,
                            IsActive = Store.CurrentApp.LicenseInformation.ProductLicenses[id].IsActive
                        }
                    );
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Loading of products failed with error: " + e.Message);
            }

            return productItems;
        }
    }
}
