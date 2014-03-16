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
                    return ScaleFactor.P720;
                case 160:
                    return ScaleFactor.WXGA;
                default:
                    return ScaleFactor.P1080;
	        }
        }
    }
}
