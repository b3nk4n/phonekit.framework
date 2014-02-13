using PhoneKit.Framework.Controls;
using PhoneKit.TestApp.Resources;
using System;
using System.Collections.Generic;

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
            // app
            ApplicationIconSource = new Uri("/Assets/ApplicationIcon.png", UriKind.Relative);
            ApplicationTitle = AppResources.ApplicationTitle;
            ApplicationVersion = AppResources.ApplicationVersion;
            ApplicationAuthor= AppResources.ApplicationAuthor;
            ApplicationDescription = AppResources.ApplicationDescription;

            // buttons
            SupportAndFeedbackText = AppResources.SupportAndFeedback;
            SupportAndFeedbackEmail = "apps@bsautermeister.de";
            PrivacyInfoText= AppResources.PrivacyInfo;
            PrivacyInfoLink= "http://bsautermeister.de/privacy.php";
            RateAndReviewText = AppResources.RateAndReview;
            MoreAppsText= AppResources.MoreApps;
            MoreAppsSearchTerms = "Benjamin Sautermeister";

            // contributors
            ContributorsListVisibility = System.Windows.Visibility.Visible;
            SetContributorsList(new List<ContributorModel>() {
                new ContributorModel("/Assets/Languages/italy.png","Max Mustermann"),
                new ContributorModel("/Assets/Languages/french.png","Benjamin Sautermeister"),
                new ContributorModel("/Assets/Languages/portuguese.png","João Vitório Dagostin")});
        }
    }
}
