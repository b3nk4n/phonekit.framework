﻿using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneKit.Framework.Advertising
{
    /// <summary>
    /// The adverts data for an adverts control.
    /// </summary>
    public class AdvertData
    {
        /// <summary>
        /// The action types when clicking the app.
        /// </summary>
        public enum ActionTypes
        {
            StoreSearchTerm,
            AppId,
            Website,
            None
        }

        /// <summary>
        /// The advert image path.
        /// </summary>
        public Uri ImageUri { get; set; }

        /// <summary>
        /// The adverts action type when clicking on it.
        /// </summary>
        public ActionTypes ActionType { get; set; }
        
        /// <summary>
        /// The action parameter string required to execute the click action.
        /// </summary>
        public string ActionParameter { get; set; }

        /// <summary>
        /// The marketplace serach task.
        /// </summary>
        private static MarketplaceSearchTask searchTask = new MarketplaceSearchTask();

        /// <summary>
        /// The marketplace serach task.
        /// </summary>
        private static MarketplaceDetailTask detailTask = new MarketplaceDetailTask();

        /// <summary>
        /// The web browser task.
        /// </summary>
        private static WebBrowserTask browserTask = new WebBrowserTask();

        /// <summary>
        /// Creates an AdvertData instance.
        /// </summary>
        /// <param name="imageUri">The advert image URI.</param>
        /// <param name="action"></param>
        /// <param name="actionParameter"></param>
        public AdvertData(Uri imageUri, ActionTypes action, string actionParameter)
        {
            ImageUri = imageUri;
            ActionType = action;
            ActionParameter = actionParameter;
        }

        /// <summary>
        /// Executes the click action.
        /// </summary>
        public void ExecuteAction()
        {
            // no action when no action info is defined
            if (string.IsNullOrEmpty(ActionParameter))
                return;

            switch (ActionType)
            {
                case ActionTypes.StoreSearchTerm:
                    searchTask.ContentType = MarketplaceContentType.Applications;
                    searchTask.SearchTerms = ActionParameter;
                    searchTask.Show();
                    break;
                case ActionTypes.AppId:
                    detailTask.ContentType = MarketplaceContentType.Applications;
                    detailTask.ContentIdentifier = ActionParameter;
                    detailTask.Show();
                    break;
                case ActionTypes.Website:
                    browserTask.Uri = new Uri(ActionParameter, UriKind.Absolute);
                    break;
            }
        }
    }
}