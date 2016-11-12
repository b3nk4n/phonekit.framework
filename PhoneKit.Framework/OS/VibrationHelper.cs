using Microsoft.Devices;
using System;

namespace PhoneKit.Framework.OS
{
    /// <summary>
    /// The vibration helper class.
    /// </summary>
    public static class VibrationHelper
    {
        /// <summary>
        /// The vibration controller.
        /// </summary>
        private static VibrateController _vibrateController = VibrateController.Default;

        /// <summary>
        /// Starts to vibrate.
        /// </summary>
        /// <param name="seconds">The time span in seconds.</param>
        public static void Vibrate(double seconds)
        {
            _vibrateController.Start(TimeSpan.FromSeconds(seconds));
        }

        /// <summary>
        /// Stops to vibrate.
        /// </summary>
        public static void Stop()
        {
            _vibrateController.Stop();
        }
    }
}
