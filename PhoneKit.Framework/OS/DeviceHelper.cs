using Microsoft.Phone.Info;

namespace PhoneKit.Framework.OS
{
    /// <summary>
    /// Helps to detect the device type
    /// </summary>
    public static class DeviceHelper
    {
        /// <summary>
        /// The Lumia 520 device names.
        /// </summary>
        private static readonly string[] LUMIA520_DEVICES = { "RM-914", "RM-915", "RM-917" };

        /// <summary>
        /// The Lumia 525 device names.
        /// </summary>
        private static readonly string[] LUMIA525_DEVICES = { "RM-998" };

        /// <summary>
        /// The Lumia 920 device names.
        /// </summary>
        private static readonly string[] LUMIA920_DEVICES = { "RM-821", "RM-820" };

        /// <summary>
        /// The Lumia 925 device names.
        /// </summary>
        private static readonly string[] LUMIA925_DEVICES = { "RM-910", "RM-893", "RM-892" };

        /// <summary>
        /// The Lumia 1020 device names.
        /// </summary>
        private static readonly string[] LUMIA1020_DEVICES = { "RM-875", "RM-877", "RM-876", "RM-893" };

        /// <summary>
        /// The Lumia 1520 device names.
        /// </summary>
        private static readonly string[] LUMIA1520_DEVICES = { "RM-937", "RM-938", "RM-939", "RM-940" };

        /// <summary>
        /// Indicates whether the phone might be a Lumia 520.
        /// </summary>
        public static bool IsLumia520
        {
            get
            {
                return HasDeviceName(LUMIA520_DEVICES);
            }
        }

        /// <summary>
        /// Indicates whether the phone might be a Lumia 525.
        /// </summary>
        public static bool IsLumia525
        {
            get
            {
                return HasDeviceName(LUMIA525_DEVICES);
            }
        }

        /// <summary>
        /// Indicates whether the phone might be a Lumia 920.
        /// </summary>
        public static bool IsLumia920
        {
            get
            {
                return HasDeviceName(LUMIA920_DEVICES);
            }
        }

        /// <summary>
        /// Indicates whether the phone might be a Lumia 925.
        /// </summary>
        public static bool IsLumia925
        {
            get
            {
                return HasDeviceName(LUMIA925_DEVICES);
            }
        }

        /// <summary>
        /// Indicates whether the phone might be a Lumia 1020.
        /// </summary>
        public static bool IsLumia1020
        {
            get
            {
                return HasDeviceName(LUMIA1020_DEVICES);
            }
        }

        /// <summary>
        /// Indicates whether the phone might be a Lumia 1520.
        /// </summary>
        public static bool IsLumia1520
        {
            get
            {
                return HasDeviceName(LUMIA1520_DEVICES);
            }
        }

        /// <summary>
        /// Checks whether the current device contains one of the given device names.
        /// </summary>
        /// <param name="deviceNames">The list of device names to compare with the one of the phone.</param>
        /// <returns>Returns TRUE for a match, else FALSE.</returns>
        private static bool HasDeviceName(string[] deviceNames)
        {
            if (deviceNames == null)
                return false;

            foreach (var deviceName in deviceNames)
            {
                if (DeviceStatus.DeviceName.ToUpper().Contains(deviceName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
