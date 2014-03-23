using Microsoft.Phone.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhoneKit.Framework.OS
{
    /// <summary>
    /// The display scale factors.
    /// </summary>
    public enum ScaleFactor
    {
        WVGA,
        HD, // 720p or 1080p
        WXGA,
    }

    /// <summary>
    /// The display screen resolution.
    /// </summary>
    public enum ScreenResolution
    {
        WVGA,
        P720,
        WXGA,
        P1080
    }

    /// <summary>
    /// Helper class to get some display info.
    /// </summary>
    public static class DisplayHelper
    {
        /// <summary>
        /// Gets the scale factor.
        /// </summary>
        /// <returns>The scale factor.</returns>
        public static ScaleFactor GetScaleFactor()
        {
            switch (Application.Current.Host.Content.ScaleFactor)
	        {
                case 100:
                    return ScaleFactor.WVGA;
		        case 150:
                    return ScaleFactor.HD;
                case 160:
                    return ScaleFactor.WXGA;
                default:
                    // if a new one is added later and this func is not
                    // updated, use the default one
                    return ScaleFactor.WVGA;
	        }
        }

        /// <summary>
        /// Gets the screen resoltion.
        /// </summary>
        /// <returns>The screen resolution.</returns>
        public static ScreenResolution GetResolution()
        {
            switch (Application.Current.Host.Content.ScaleFactor)
            {
                case 100:
                    return ScreenResolution.WVGA;
                case 150:
                    object temp;
                    if (DeviceExtendedProperties.TryGetValue("PhysicalScreenResolution", out temp))
                        return ScreenResolution.P1080;
                    else
                        return ScreenResolution.P720;
                case 160:
                    return ScreenResolution.WXGA;
                default:
                    // if a new one is added later and this func is not
                    // updated, use the default one
                    return ScreenResolution.WVGA;
            }
        }
    }
}
