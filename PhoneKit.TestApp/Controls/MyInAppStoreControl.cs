using PhoneKit.Framework.Controls;
using PhoneKit.TestApp.Resources;

namespace PhoneKit.TestApp.Controls
{
    /// <summary>
    /// The localized in-app store control.
    /// </summary>
    class MyInAppStoreControl : InAppStoreControlBase
    {
        /// <summary>
        /// Localizes the user control content and texts.
        /// </summary>
        protected override void LocalizeContent()
        {
            InAppStoreLoadingText = AppResources.InAppStoreLoading;
            InAppStoreNoProductsText = AppResources.InAppStoreNoProducts;
            InAppStorePurchasedText = AppResources.InAppStorePurchased;
            SupportedProductIds = "ad_free,donate";
        }
    }
}
