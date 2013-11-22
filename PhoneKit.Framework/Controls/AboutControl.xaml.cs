using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Tasks;
using PhoneKit.Framework.OS;

namespace PhoneKit.Framework.Controls
{
    /// <summary>
    /// The user control for the default about page.
    /// </summary>
    public partial class AboutControl : UserControl
    {
        #region Members

        /// <summary>
        /// The applications version.
        /// </summary>
        private string _applicationVersion = "v 1.0";

        /// <summary>
        /// The localized application title as a dependency property.
        /// </summary>
        public static readonly DependencyProperty ApplicationTitleProperty =
            DependencyProperty.Register("ApplicationTitle",
            typeof(string), typeof(AboutControl),
            new PropertyMetadata("Application"));

        /// <summary>
        /// The application icons source.
        /// </summary>
        private string _applicationIconSource = "/Assets/ApplicationIcon.png";

        /// <summary>
        /// The localized application author text as a dependency property.
        /// </summary>
        public static readonly DependencyProperty ApplicationAuthorProperty =
            DependencyProperty.Register("AppllicationAuthor",
            typeof(string), typeof(AboutControl),
            new PropertyMetadata("by Benjamin Sautermeister"));

        /// <summary>
        /// The localized application description text as a dependency property.
        /// </summary>
        public static readonly DependencyProperty ApplicationDescriptionProperty =
            DependencyProperty.Register("AppllicationDescription",
            typeof(string), typeof(AboutControl),
            new PropertyMetadata("This application makes your life more easy."));

        /// <summary>
        /// The localized support and fedback text as a dependency property.
        /// </summary>
        public static readonly DependencyProperty SupportAndFeedbackTextProperty =
            DependencyProperty.Register("SupportAndFeedbackText",
            typeof(string), typeof(AboutControl),
            new PropertyMetadata("support and feedback"));

        /// <summary>
        /// The support mail adress.
        /// </summary>
        private string _supportAndFeedbackEmail = string.Empty;

        /// <summary>
        /// The localized privacy info text as a dependency property.
        /// </summary>
        public static readonly DependencyProperty PrivacyInfoTextProperty =
            DependencyProperty.Register("PrivactInfoText",
            typeof(string), typeof(AboutControl),
            new PropertyMetadata("privacy info"));

        /// <summary>
        /// The privacy info link.
        /// </summary>
        private string _privacyInfoLink = string.Empty;

        /// <summary>
        /// The localized rate and review text as a dependency property.
        /// </summary>
        public static readonly DependencyProperty RateAndReviewTextProperty =
            DependencyProperty.Register("RateAndReviewText",
            typeof(string), typeof(AboutControl),
            new PropertyMetadata("rate and review"));

        /// <summary>
        /// The localized more apps text as a dependency property.
        /// </summary>
        public static readonly DependencyProperty MoreAppsTextProperty =
            DependencyProperty.Register("MoreAppsText",
            typeof(string), typeof(AboutControl),
            new PropertyMetadata("more apps"));

        /// <summary>
        /// The more apps store search term.
        /// </summary>
        private string _moreAppsSearchTerms = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an AboutControl instance.
        /// </summary>
        public AboutControl()
        {
            InitializeComponent();

            SetFrameworkBranding();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the framework branding.
        /// </summary>
        private void SetFrameworkBranding()
        {
            Branding.Text = "Created with PhoneKit Framwork v " + VersionHelper.GetFrameworkVersionText();
        }

        #endregion

        #region Events

        /// <summary>
        /// Called when the support button was clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void SupportAndFeedback_Click(object sender, RoutedEventArgs e)
        {
            var emailTask = new EmailComposeTask();
            emailTask.To = SupportAndFeedbackEmail;
            emailTask.Subject = string.Format("[{0}] ", ApplicationTitle);
            emailTask.Show();
        }

        /// <summary>
        /// Called when the privacy button was clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void PrivacyInfo_Click(object sender, RoutedEventArgs e)
        {
            var browserTask = new WebBrowserTask();
            browserTask.Uri = new Uri(PrivacyInfoLink, UriKind.Absolute);
            browserTask.Show();
        }

        /// <summary>
        /// Called when the rate/review button was clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void RateAndReview_Click(object sender, RoutedEventArgs e)
        {
            var reviewTask = new MarketplaceReviewTask();
            reviewTask.Show();
        }

        /// <summary>
        /// Called when the more apps button was clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void MoreApps_Click(object sender, RoutedEventArgs e)
        {
            var searchTask = new MarketplaceSearchTask();
            searchTask.SearchTerms = MoreAppsSearchTerms;
            searchTask.ContentType = MarketplaceContentType.Applications;
            searchTask.Show();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the applications version.
        /// </summary>
        public string ApplicationVersion
        {
            get
            {
                return _applicationVersion;
            }
            set
            {
                _applicationVersion = value;
            }
        }

        /// <summary>
        /// Gets or sets the application title.
        /// </summary>
        public string ApplicationTitle
        {
            get { return (string)GetValue(ApplicationTitleProperty); }
            set { SetValue(ApplicationTitleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the application icons source.
        /// </summary>
        public string ApplicationIconSource
        {
            get
            {
                return _applicationIconSource;
            }
            set
            {
                _applicationIconSource = value;
            }
        }

        /// <summary>
        /// Gets or sets the applications author text.
        /// </summary>
        public string ApplicationAuthor
        {
            get { return (string)GetValue(ApplicationAuthorProperty); }
            set { SetValue(ApplicationAuthorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the applications description text.
        /// </summary>
        public string ApplicationDescription
        {
            get { return (string)GetValue(ApplicationDescriptionProperty); }
            set { SetValue(ApplicationDescriptionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the support and feedback text.
        /// </summary>
        public string SupportAndFeedbackText
        {
            get { return (string)GetValue(SupportAndFeedbackTextProperty); }
            set { SetValue(SupportAndFeedbackTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the support email.
        /// </summary>
        public string SupportAndFeedbackEmail
        {
            get
            {
                return _supportAndFeedbackEmail;
            }
            set
            {
                _supportAndFeedbackEmail = value;
            }
        }

        /// <summary>
        /// Gets or sets the privacy info text.
        /// </summary>
        public string PrivacyInfoText
        {
            get { return (string)GetValue(PrivacyInfoTextProperty); }
            set { SetValue(PrivacyInfoTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the privacy info link.
        /// </summary>
        public string PrivacyInfoLink
        {
            get
            {
                return _privacyInfoLink;
            }
            set
            {
                _privacyInfoLink = value;
            }
        }

        /// <summary>
        /// Gets or sets the rate and review text.
        /// </summary>
        public string RateAndReviewText
        {
            get { return (string)GetValue(RateAndReviewTextProperty); }
            set { SetValue(RateAndReviewTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the more apps text.
        /// </summary>
        public string MoreAppsText
        {
            get { return (string)GetValue(MoreAppsTextProperty); }
            set { SetValue(MoreAppsTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the more apps store search term.
        /// </summary>
        public string MoreAppsSearchTerms
        {
            get
            {
                return _moreAppsSearchTerms;
            }
            set
            {
                _moreAppsSearchTerms = value;
            }
        }
        
        #endregion
    }
}
