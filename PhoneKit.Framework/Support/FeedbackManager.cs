using PhoneKit.Framework.Core.Storage;

namespace PhoneKit.Framework.Support
{
    /// <summary>
    /// The feedback status.
    /// </summary>
    public enum FeedbackState
    {
        Inactive,
        FirstReview,
        SecondReview,
        Feedback
    }

    /// <summary>
    /// Controls the behaviour of the FeedbackDialogControl. When the app has been launched 5 times the 
    /// initial prompt is shown. If the user reviews no more prompts are shown. When the app has been
    /// launched 10 times and not been reviewed, the prompt is shown.
    /// </summary>
    public class FeedbackManager
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        private static FeedbackManager _instance;

        /// <summary>
        /// The persistent launch counter.
        /// </summary>
        private readonly StoredObject<int> _launchCount = new StoredObject<int>("_launch_count_", 0);

        /// <summary>
        /// The persistent reviewed flag.
        /// </summary>
        private readonly StoredObject<bool> _reviewed = new StoredObject<bool>("_reviewed_", false);

        /// <summary>
        /// The feedback state.
        /// </summary>
        private FeedbackState _state = FeedbackState.Inactive;

        /// <summary>
        /// Creates a FeedbackManager instance.
        /// </summary>
        private FeedbackManager()
        {
        }

        /// <summary>
        /// Launches the first review question.
        /// </summary>
        public void StartFirst()
        {
            var license = new Microsoft.Phone.Marketplace.LicenseInformation();

            // Only load state if not trial
            if (!license.IsTrial())
            {
                if (!_reviewed.Value)
                {
                    this._state = FeedbackState.FirstReview;
                }
            }
        }

        /// <summary>
        /// Launches the second review question.
        /// </summary>
        public void StartSecond()
        {
            var license = new Microsoft.Phone.Marketplace.LicenseInformation();

            // Only load state if not trial
            if (!license.IsTrial())
            {
                if (!_reviewed.Value)
                {
                    this._state = FeedbackState.SecondReview;
                }
            }
        }

        /// <summary>
        /// Call when user has reviewed
        /// </summary>
        public void NotifyReviewed()
        {
            this._reviewed.Value = true;
        }

        /// <summary>
        /// Gets the feedback manager instance.
        /// </summary>
        public static FeedbackManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FeedbackManager();
                return _instance;
            }
        }

        /// <summary>
        /// Gets or sets the feedback state.
        /// </summary>
        public FeedbackState State
        {
            get { return this._state; }
            set { this._state = value; }
        }
    }
}
