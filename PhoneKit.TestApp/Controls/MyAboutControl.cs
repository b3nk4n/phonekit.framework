using PhoneKit.Framework.Controls;
using PhoneKit.TestApp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhoneKit.TestApp.Controls
{
    public class MyAboutControl : AboutControlBase
    {
        protected override void LocalizeContent()
        {
            ApplicationIconSource = new Uri("/Assets/ApplicationIcon.png", UriKind.Relative);
            ApplicationTitle = AppResources.ApplicationTitle;
            ApplicationVersion = "v 1.1";
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
