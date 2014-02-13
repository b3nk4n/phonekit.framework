using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Tasks;
using PhoneKit.Framework.OS;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace PhoneKit.Framework.Controls
{
    /// <summary>
    /// The user control for the default about page.
    /// </summary>
    public abstract partial class AboutControlBase : UserControl
    {
        #region Members

        /// <summary>
        /// The support mail adress.
        /// </summary>
        private string _supportAndFeedbackEmail = string.Empty;

        /// <summary>
        /// The privacy info link.
        /// </summary>
        private string _privacyInfoLink = string.Empty;

        /// <summary>
        /// The more apps store search term.
        /// </summary>
        private string _moreAppsSearchTerms = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an AboutControl instance.
        /// </summary>
        public AboutControlBase()
        {
            InitializeComponent();
            LocalizeContent();
            SetFrameworkBranding();

            // Note: register events in code and not in XAML because this
            //       is not allowed when inheritance is used
            SupportAndFeedbackElement.Click += (s, e) =>
                {
                    var emailTask = new EmailComposeTask();
                    emailTask.To = SupportAndFeedbackEmail;
                    emailTask.Subject = string.Format("[{0}] ", ApplicationTitleElement.Text);
                    emailTask.Show();
                };
            PrivacyInfoElement.Click += (s, e) =>
                {
                    var browserTask = new WebBrowserTask();
                    browserTask.Uri = new Uri(PrivacyInfoLink, UriKind.Absolute);
                    browserTask.Show();
                };
            RateAndReviewElement.Click += (s, e) =>
                {
                    var reviewTask = new MarketplaceReviewTask();
                    reviewTask.Show();
                };
            MoreAppsElement.Click += (s, e) =>
                {
                    var searchTask = new MarketplaceSearchTask();
                    searchTask.SearchTerms = MoreAppsSearchTerms;
                    searchTask.ContentType = MarketplaceContentType.Applications;
                    searchTask.Show();
                };
        }

        /// <summary>
        /// Sets the contributors to the list.
        /// </summary>
        /// <remarks>
        /// Do not forget <code>ContributorsListVisibility = Visible;</code>
        /// </remarks>
        /// <param name="contributors"></param>
        public void SetContributorsList(IEnumerable<ContributorModel> contributors)
        {
            ContributorsList.ItemsSource = contributors;
        }

        /// <summary>
        /// Localizes the user control content and texts.
        /// </summary>
        protected abstract void LocalizeContent();

        #endregion

        #region Methods

        /// <summary>
        /// Sets the framework branding.
        /// </summary>
        private void SetFrameworkBranding()
        {
            BrandingElement.Text = "powered by PhoneKit Framework " + VersionHelper.GetFrameworkVersionText();
        }

        #endregion

        #region Properties

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

        /// <summary>
        /// Sets the application icons source.
        /// </summary>
        public Uri ApplicationIconSource
        {
            set
            {
                ApplicationIconElement.Source = new BitmapImage(value);
            }
        }

        /// <summary>
        /// Gets or sets the application title.
        /// </summary>
        public string ApplicationTitle
        {
            set
            {
                ApplicationTitleElement.Text = value;
            }
        }

        /// <summary>
        /// Sets the application author.
        /// </summary>
        public string ApplicationAuthor
        {
            set
            {
                ApplicationAuthorElement.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the application version.
        /// </summary>
        public string ApplicationVersion
        {
            set
            {
                ApplicationVersionElement.Text = value;
            }
        }

        /// <summary>
        /// Sets the application description.
        /// </summary>
        public string ApplicationDescription
        {
            set
            {
                ApplicationDescriptionElement.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the support and feedback button text.
        /// </summary>
        public string SupportAndFeedbackText
        {
            set
            {
                SupportAndFeedbackElement.Content = value;
            }
        }

        /// <summary>
        /// Sets the privacy info button text.
        /// </summary>
        public string PrivacyInfoText
        {
            set
            {
                PrivacyInfoElement.Content = value;
            }
        }

        /// <summary>
        /// Sets the rate and review button text.
        /// </summary>
        public string RateAndReviewText
        {
            set
            {
                RateAndReviewElement.Content = value;
            }
        }

        /// <summary>
        /// Sets the more apps button text.
        /// </summary>
        public string MoreAppsText
        {
            set
            {
                MoreAppsElement.Content = value;
            }
        }

        /// <summary>
        /// Sets the contributors list visiblility to show or hide the contributors list.
        /// </summary>
        public Visibility ContributorsListVisibility
        {
            set
            {
                ContributorsContainer.Visibility = value;
            }
        }
        
        #endregion
    }
}
