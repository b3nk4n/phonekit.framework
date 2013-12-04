using Microsoft.Phone.Tasks;
using PhoneKit.Framework.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhoneKit.Framework.Core.Support
{
    /// <summary>
    /// Exception logger and reporter.
    /// </summary>
    public class ErrorReportingManager
    {
        /// <summary>
        /// The exception log file name.
        /// </summary>
        public const string EXCEPTION_LOG_FILE_NAME = "exceptionLogger.log";

        /// <summary>
        /// The singleton instance.
        /// </summary>
        private static ErrorReportingManager _instance;

        /// <summary>
        /// Creates a ExceptionLogger.
        /// </summary>
        private ErrorReportingManager()
        { }

        /// <summary>
        /// Gets the exception logger instance.
        /// </summary>
        public static ErrorReportingManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ErrorReportingManager();
                return _instance;
            }
        }

        /// <summary>
        /// Logs an unhandled exception.
        /// </summary>
        /// <remarks>
        /// Use this method inside <code>RootFrame_NavigationFailed</code> and <code>Application_UnhandledException</code>
        /// before the <code>Debugger.IsAttached</code> checks in the App.cs file.
        /// </remarks>
        /// <param name="e">The exception.</param>
        public void Save(Exception e)
        {
            StorageHelper.DeleteFile(EXCEPTION_LOG_FILE_NAME);

            var logData = new ErrorReport(e);

            StorageHelper.SaveFile<ErrorReport>(EXCEPTION_LOG_FILE_NAME, logData);
        }

        /// <summary>
        /// Checks and reports an unhandled exception of the previous run via email.
        /// </summary>
        /// <remarks>
        /// Use this method at the end of <code>CompleteInitializePhoneApplication</code>
        /// in the App.cs file.
        /// </remarks>
        /// <param name="supportEmail">The support email address.</param>
        /// <param name="subject">The email subject. Plese use formats like: [APPNAME]</param>
        public void CheckAndReport(string supportEmail, string subject)
        {
            if (!StorageHelper.FileExists(EXCEPTION_LOG_FILE_NAME))
                return;

            ErrorReport content = StorageHelper.LoadFile<ErrorReport>(EXCEPTION_LOG_FILE_NAME);

            if (MessageBox.Show("A problem occurred the last time you ran this application. Would you like to send an email to report it?",
                "Problem Report", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                EmailComposeTask email = new EmailComposeTask();
                email.To = supportEmail;
                email.Subject = subject;
                email.Body = content.ToString();
                email.Show();
            }

            StorageHelper.DeleteFile(EXCEPTION_LOG_FILE_NAME);
        }
    }
}
