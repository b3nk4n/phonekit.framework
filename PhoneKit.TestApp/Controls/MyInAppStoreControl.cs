using PhoneKit.Framework.Controls;
using PhoneKit.TestApp.Resources;
using System.Windows.Media;

namespace PhoneKit.TestApp.Controls
{
    /// <summary>
    /// The localized in-app store control.
    /// </summary>
    public class MyInAppStoreControl : InAppStoreControlBase
    {
        public MyInAppStoreControl()
        {
            BackgroundTheme = new SolidColorBrush(Colors.Yellow);
        }

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
