using Microsoft.Phone.Scheduler;
using System;
using System.Windows;

namespace PhoneKit.Framework.Tasks
{
    /// <summary>
    /// Helper class to manage an applications background task.
    /// </summary>
    public static class BackgroundTaskHelper
    {
        /// <summary>
        /// Checks whether a background task is active.
        /// </summary>
        /// <param name="taskName">The background tasks name.</param>
        /// <returns>Returns true if the background task is scheduled, else false.</returns>
        public static bool IsTaskActive(string taskName)
        {
            var task = ScheduledActionService.Find(taskName);
            return task != null && task.IsScheduled;
        }

        /// <summary>
        /// Starts a new task in the background, if the application has required privilegs.
        /// </summary>
        /// <param name="task">The task to schedule.</param>
        /// <returns>Returns true if the scheduling of the task was successful, else false.</returns>
        public static bool StartTask(ScheduledTask task)
        {
            // if the task already exists and background agents are enabled for the application,
            // you must remove the task and then add it again to update the schedule
            if (task != null)
            {
                RemoveTask(task.Name);
            }

            // place the call to Add in a try block in case the user has disabled agents
            try
            {
                ScheduledActionService.Add(task);

                // if debugging is enabled, use LaunchForTest to launch the agent in one minute
#if(DEBUG)
                ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromSeconds(60));
#endif
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("BNS Error: The action is disabled"))
                {
                    MessageBox.Show("Background agents for this application have been disabled.");
                }

                if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
                {
                    // no user action required. The system prompts the user when the hard limit of periodic tasks has been reached
                }
                return false;
            }
            catch (SchedulerServiceException)
            {
                // no user action required
                return false;
            }

            return true;
        }

        /// <summary>
        /// Removes the background task.
        /// </summary>
        /// <param name="name">The background tasks name.</param>
        public static void RemoveTask(string name)
        {
            try
            {
                var service = ScheduledActionService.Find(name);

                if (service != null)
                    ScheduledActionService.Remove(name);
            }
            catch (Exception)
            {
                // no user action required
            }
        }
    }
}
