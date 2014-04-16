using Microsoft.Phone.Info;
using System;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace PhoneKit.Framework.Support
{
    /// <summary>
    /// The data of an exception log.
    /// </summary>
    [DataContract]
    internal class ErrorReport
    {
        /// <summary>
        /// Gets or sets the tag as an extra info text.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the exception message.
        /// </summary>
        [DataMember(Name = "message")]
        private string Message { get; set; }

        /// <summary>
        /// Gets or sets the stack trace of the exception.
        /// </summary>
        [DataMember(Name = "stack_trace")]
        private string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the time of the logged exception.
        /// </summary>
        [DataMember(Name = "time")]
        private DateTime Time { get; set; }

        /// <summary>
        /// Gets or sets the application version of the logged exception.
        /// </summary>
        [DataMember(Name = "applicationVersion")]
        private string ApplicationVersion { get; set; }

        /// <summary>
        /// Gets or sets the application language of the logged exception.
        /// </summary>
        [DataMember(Name = "applicationLanguage")]
        private string ApplicationLanguage { get; set; }

        /// <summary>
        /// Creates an empty log data object.
        /// </summary>
        public ErrorReport()
        {
            Type = "Exception";
            Message = string.Empty;
            StackTrace = string.Empty;
            Time = DateTime.Now;
            ApplicationVersion = string.Empty;
            ApplicationLanguage = string.Empty;
        }

        /// <summary>
        /// Creates a new exception log data object.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="applicationVersion">The application version.</param>
        /// <param name="applicationLanguage">The application language.</param>
        public ErrorReport(Exception exception, string applicationVersion, string applicationLanguage)
        {
            Type = exception.GetType().Name;
            Message = exception.Message;
            StackTrace = exception.StackTrace;
            Time = DateTime.Now;
            ApplicationVersion = applicationVersion;
            ApplicationLanguage = applicationLanguage;
        }

        /// <summary>
        /// Gets the string representation of a log data in XML format.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<error>");
            sb.AppendFormat("<tag>\n{0}\n</tag>\n", Type);
            sb.AppendFormat("<time>\n{0:u}\n</time>\n", Time);
            sb.AppendFormat("<message>\n{0}\n</message>\n", Message);
            sb.AppendFormat("<trace>\n{0}\n</trace>\n", StackTrace);
            sb.AppendFormat("<appVersion>\n{0}\n</appVersion>\n", ApplicationVersion);
            sb.AppendFormat("<appLanguage>\n{0}\n</appLanguage>\n", ApplicationLanguage);
            sb.AppendFormat("<deviceName>\n{0}\n</deviceName>\n", DeviceStatus.DeviceName);
            sb.AppendFormat("<deviceManufacturer>\n{0}\n</deviceManufacturer>\n", DeviceStatus.DeviceManufacturer);
            sb.AppendFormat("<deviceFirmwareVersion>\n{0}\n</deviceFirmwareVersion>\n", DeviceStatus.DeviceFirmwareVersion);
            sb.AppendFormat("<deviceHardwareVersion>\n{0}\n</deviceHardwareVersion>\n", DeviceStatus.DeviceHardwareVersion);
            sb.AppendLine("</error>");
            return sb.ToString();
        }
    }
}
