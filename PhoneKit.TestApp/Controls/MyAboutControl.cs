using PhoneKit.Framework.Controls;
using PhoneKit.TestApp.Resources;
using System;

namespace PhoneKit.TestApp.Controls
{
    /// <summary>
    /// The localized About control.
    /// </summary>
    public class MyAboutControl : AboutControlBase
    {
        /// <summary>
        /// Localizes the user controls contents and texts.
        /// </summary>
        protected override void LocalizeContent()
        {
            ApplicationIconSource = new Uri("/Assets/ApplicationIcon.png", UriKind.Relative);
            ApplicationTitle = AppResources.ApplicationTitle;
            ApplicationVersion = AppResources.ApplicationVersion;
            ApplicationAuthor= AppResources.ApplicationAuthor;
            ApplicationDescription = AppResources.ApplicationDescription;
            SupportAndFeedbackText = AppResources.SupportAndFeedback;
            SupportAndFeedbackEmail = "apps@bsautermeister.de";
            PrivacyInfoText= AppResources.PrivacyInfo;
            PrivacyInfoLink= "http://bsautermeister.de/privacy.php";
            RateAndReviewText = AppResources.RateAndReview;
            MoreAppsText= AppResources.MoreApps;
            MoreAppsSearchTerms = "Benjamin Sautermeister";
        }
    }
}
