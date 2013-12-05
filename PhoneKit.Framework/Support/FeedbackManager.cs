using PhoneKit.Framework.Core.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// The number of start ups for the first rating request.
        /// </summary>
        private const int FIRST_COUNT = 5;

        /// <summary>
        /// The number of start ups for the second rating request.
        /// </summary>
        private const int SECOND_COUNT = 10;

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static FeedbackManager _instance;

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
        /// This should only be called when the app is Launching
        /// </summary>
        public void Launching()
        {
            var license = new Microsoft.Phone.Marketplace.LicenseInformation();

            // Only load state if not trial
            if (!license.IsTrial())
                this.LoadState();
        }

        /// <summary>
        /// Loads last state from storage and works out the new state
        /// </summary>
        private void LoadState()
        {
            try
            {
                if (!_reviewed.Value)
                {
                    _launchCount.Value++;

                    if (_launchCount.Value == FIRST_COUNT)
                        this._state = FeedbackState.FirstReview;
                    else if (_launchCount.Value == SECOND_COUNT)
                        this._state = FeedbackState.SecondReview;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("FeedbackHelper.LoadState - Failed to load state, Exception: {0}", ex.ToString()));
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
