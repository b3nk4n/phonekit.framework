using PhoneKit.Framework.Controls;
using PhoneKit.TestApp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneKit.TestApp.Controls
{
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
