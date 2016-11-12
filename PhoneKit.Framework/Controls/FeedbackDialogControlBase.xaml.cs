using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using PhoneKit.Framework.Support;
using Microsoft.Phone.Info;

namespace PhoneKit.Framework.Controls
{
    /// <summary>
    /// The base feedback dialog control.
    /// </summary>
    public abstract partial class FeedbackDialogControlBase : UserControl
    {
        #region Members

        /// <summary>
        /// Use this for detecting visibility change on code
        /// </summary>
        public event EventHandler VisibilityChanged = null;

        /// <summary>
        /// The applications root frame.
        /// </summary>
        private PhoneApplicationFrame _rootFrame = null;

        /// <summary>
        /// Gets or sets the application version.
        /// </summary>
        public string ApplicationVersion { get; set; }

        /// <summary>
        /// Gets or sets the rating title text.
        /// </summary>
        public string RatingTitleText { get; set; }

        /// <summary>
        /// Gets or sets the rating message text at 5 starts.
        /// </summary>
        public string RatingMessage5Text { get; set; }

        /// <summary>
        /// Gets or sets the rating message text at 10 starts.
        /// </summary>
        public string RatingMessage10Text { get; set; }

        /// <summary>
        /// Gets or sets the YES rating button text.
        /// </summary>
        public string RatingYesText { get; set; }

        /// <summary>
        /// Gets or sets the NO rating button text.
        /// </summary>
        public string RatingNoText { get; set; }

        /// <summary>
        /// Gets or sets the feedback title text.
        /// </summary>
        public string FeedbackTitleText { get; set; }

        /// <summary>
        /// Gets or sets the feedback message text.
        /// </summary>
        public string FeedbackMessageText { get; set; }

        /// <summary>
        /// Gets or sets the feedback email address.
        /// </summary>
        public string FeedbackEmail { get; set; }

        /// <summary>
        /// Gets or sets the feedback email subject.
        /// </summary>
        public string FeedbackSubject { get; set; }

        /// <summary>
        /// Gets or sets the feedback body text.
        /// </summary>
        public string FeedbackBodyText { get; set; }

        /// <summary>
        /// Gets or sets the YES feedback button text.
        /// </summary>
        public string FeedbackYesText { get; set; }

        /// <summary>
        /// Gets or sets the NO feedback button text.
        /// </summary>
        public string FeedbackNoText { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a FeedbackDialogControlBase instance.
        /// </summary>
        public FeedbackDialogControlBase()
        {
            InitializeComponent();
            LocalizeContent();

            this.yesButton.Click += YesButton_Click;
            this.noButton.Click += (s, e) =>
                {
                    OnNoClick();
                };
            this.Loaded += FeedbackDialog_Loaded;
            this.hideContent.Completed += (s, e) =>
                {
                    this.ShowFeedback();
                };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Localizes the 
        /// </summary>
        protected abstract void LocalizeContent();

        /// <summary>
        /// Loaded event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void FeedbackDialog_Loaded(object sender, RoutedEventArgs e)
        {
            this.AttachBackKeyPressed();

            if (FeedbackDialogControlBase.GetEnableAnimation(this))
            {
                this.LayoutRoot.Opacity = 0;
                this.xProjection.RotationX = 90;
            }

            if (FeedbackManager.Instance.State == FeedbackState.FirstReview)
            {
                this.SetVisibility(true);
                this.SetupFirstMessage();

                if (FeedbackDialogControlBase.GetEnableAnimation(this))
                    this.showContent.Begin();
            }
            else if (FeedbackManager.Instance.State == FeedbackState.SecondReview)
            {
                this.SetVisibility(true);
                this.SetupSecondMessage();

                if (FeedbackDialogControlBase.GetEnableAnimation(this))
                    this.showContent.Begin();
            }
            else
            {
                this.SetVisibility(false);
            }
        }

        /// <summary>
        /// Attaches the back button pressed event.
        /// </summary>
        private void AttachBackKeyPressed()
        {
            // Detect back pressed
            if (this._rootFrame == null)
            {
                this._rootFrame = Application.Current.RootVisual as PhoneApplicationFrame;
                this._rootFrame.BackKeyPress += (s, e) =>
                    {
                        // If back is pressed whilst open, close and cancel back to stop app exiting
                        if (this.Visibility == System.Windows.Visibility.Visible)
                        {
                            this.OnNoClick();
                            e.Cancel = true;
                        }
                    };
            }
        }

        /// <summary>
        /// Sets up the first review request dialog.
        /// </summary>
        private void SetupFirstMessage()
        {
            this.Title = RatingTitleText;
            this.Message = RatingMessage5Text;
            this.YesText = RatingYesText;
            this.NoText = RatingNoText;
        }

        /// <summary>
        /// Sets up the second review request dialog.
        /// </summary>
        private void SetupSecondMessage()
        {
            this.Title = RatingTitleText;
            this.Message = RatingMessage10Text;
            this.YesText = RatingYesText;
            this.NoText = RatingNoText;
        }

        /// <summary>
        /// Sets up the feedback message request dialog.
        /// </summary>
        private void SetupFeedbackMessage()
        {
            this.Title = FeedbackTitleText;
            this.Message = FeedbackMessageText;
            this.YesText = FeedbackYesText;
            this.NoText = FeedbackNoText;
        }

        /// <summary>
        /// Handles the NO click event.
        /// </summary>
        private void OnNoClick()
        {
            if (FeedbackDialogControlBase.GetEnableAnimation(this))
                this.hideContent.Begin();
            else
                this.ShowFeedback();
        }

        /// <summary>
        /// Shows feedback.
        /// </summary>
        private void ShowFeedback()
        {
            if (FeedbackManager.Instance.State == FeedbackState.FirstReview)
            {
                this.SetupFeedbackMessage();
                FeedbackManager.Instance.State = FeedbackState.Feedback;

                if (FeedbackDialogControlBase.GetEnableAnimation(this))
                    this.showContent.Begin();
            }
            else
            {
                FeedbackManager.Instance.State = FeedbackState.Inactive;
                this.SetVisibility(false);
            }
        }

        /// <summary>
        /// The yes button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            this.SetVisibility(false);

            if (FeedbackManager.Instance.State == FeedbackState.FirstReview)
            {
                this.Review();
            }
            else if (FeedbackManager.Instance.State == FeedbackState.SecondReview)
            {
                this.Review();
            }
            else if (FeedbackManager.Instance.State == FeedbackState.Feedback)
            {
                this.Feedback();
            }
        }

        /// <summary>
        /// Reviews the application via a task launcher.
        /// </summary>
        private void Review()
        {
            FeedbackManager.Instance.NotifyReviewed();

            var marketplace = new MarketplaceReviewTask();
            marketplace.Show();
        }

        /// <summary>
        /// Sends feedback via email task launcher.
        /// </summary>
        private void Feedback()
        {
            // Body text including hardware, firmware and software info
            string body = string.Format(FeedbackBodyText,
                DeviceStatus.DeviceName,
                DeviceStatus.DeviceManufacturer,
                DeviceStatus.DeviceFirmwareVersion,
                DeviceStatus.DeviceHardwareVersion,
                ApplicationVersion);

            // Email task
            var email = new EmailComposeTask();
            email.To = FeedbackEmail;
            email.Subject = FeedbackSubject;
            email.Body = body;

            email.Show();
        }

        /// <summary>
        /// Sets the visibility.
        /// </summary>
        /// <param name="visible">The new visibility.</param>
        private void SetVisibility(bool visible)
        {
            if (visible)
            {
                FeedbackDialogControlBase.SetIsVisible(this, true);
                FeedbackDialogControlBase.SetIsNotVisible(this, false);
                this.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                FeedbackDialogControlBase.SetIsVisible(this, false);
                FeedbackDialogControlBase.SetIsNotVisible(this, true);
                this.Visibility = System.Windows.Visibility.Collapsed;
            }

            this.OnVisibilityChanged();
        }

        /// <summary>
        /// Fires the visibility changed event.
        /// </summary>
        private void OnVisibilityChanged()
        {
            if (this.VisibilityChanged != null)
                this.VisibilityChanged(this, new EventArgs());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the dialogs title.
        /// </summary>
        private string Title
        {
            set
            {
                if (this.title.Text != value)
                {
                    this.title.Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the dialogs message.
        /// </summary>
        private string Message
        {
            set
            {
                if (this.message.Text != value)
                {
                    this.message.Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the NO text.
        /// </summary>
        private string NoText
        {
            set
            {
                if ((string)this.noButton.Content != value)
                {
                    this.noButton.Content = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the YES text.
        /// </summary>
        private string YesText
        {
            set
            {
                if ((string)this.yesButton.Content != value)
                {
                    this.yesButton.Content = value;
                }
            }
        }

        #endregion

        // Use this from XAML to control whether animation is on or off
        #region EnableAnimation Dependency Property

        public static readonly DependencyProperty EnableAnimationProperty =
            DependencyProperty.Register("EnableAnimation", typeof(bool), typeof(FeedbackDialogControlBase), new PropertyMetadata(true, null));

        public static void SetEnableAnimation(FeedbackDialogControlBase element, bool value)
        {
            element.SetValue(EnableAnimationProperty, value);
        }

        public static bool GetEnableAnimation(FeedbackDialogControlBase element)
        {
            return (bool)element.GetValue(EnableAnimationProperty);
        }

        #endregion

        // Use this for MVVM binding IsVisible
        #region IsVisible Dependency Property

        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register("IsVisible", typeof(bool), typeof(FeedbackDialogControlBase), new PropertyMetadata(false, null));

        public static void SetIsVisible(FeedbackDialogControlBase element, bool value)
        {
            element.SetValue(IsVisibleProperty, value);
        }

        public static bool GetIsVisible(FeedbackDialogControlBase element)
        {
            return (bool)element.GetValue(IsVisibleProperty);
        }

        #endregion

        // Use this for MVVM binding IsNotVisible
        #region IsNotVisible Dependency Property

        public static readonly DependencyProperty IsNotVisibleProperty =
            DependencyProperty.Register("IsNotVisible", typeof(bool), typeof(FeedbackDialogControlBase), new PropertyMetadata(true, null));

        public static void SetIsNotVisible(FeedbackDialogControlBase element, bool value)
        {
            element.SetValue(IsNotVisibleProperty, value);
        }

        public static bool GetIsNotVisible(FeedbackDialogControlBase element)
        {
            return (bool)element.GetValue(IsNotVisibleProperty);
        }

        #endregion
    }
}
