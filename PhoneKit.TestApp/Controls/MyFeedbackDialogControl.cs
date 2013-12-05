using PhoneKit.Framework.Controls;
using PhoneKit.TestApp.Resources;

namespace PhoneKit.TestApp.Controls
{
    /// <summary>
    /// The localized feedback dialog.
    /// </summary>
    public class MyFeedbackDialogControl : FeedbackDialogControlBase
    {
        /// <summary>
        /// Localizes the user controls content and texts
        /// </summary>
        protected override void LocalizeContent()
        {
            RatingTitleText = AppResources.RatingTitleText;
            RatingMessage5Text = AppResources.RatingMessage5Text;
            RatingMessage10Text = AppResources.RatingMessage10Text;
            RatingYesText = AppResources.RatingYesText;
            RatingNoText = AppResources.RatingNoText;
            FeedbackTitleText = AppResources.FeedbackTitleText;
            FeedbackMessageText = AppResources.FeedbackMessageText;
            FeedbackEmail = AppResources.FeedbackEmail;
            FeedbackSubject = AppResources.FeedbackSubject;
            FeedbackBodyText = AppResources.FeedbackBodyText;
            FeedbackYesText = AppResources.FeedbackYesText;
            FeedbackNoText = AppResources.FeedbackNoText;
            ApplicationVersion = AppResources.ApplicationVersion;
        }
    }
}
