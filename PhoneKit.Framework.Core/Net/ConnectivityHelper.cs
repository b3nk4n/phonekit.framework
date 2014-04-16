using Microsoft.Phone.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneKit.Framework.Core.Net
{
    /// <summary>
    /// Helper class for connectivity checks.
    /// </summary>
    public static class ConnectivityHelper
    {
        /// <summary>
        /// Gets whether the phone might be in airplane mode.
        /// </summary>
        /// <remarks>The result is not very reliable.</remarks>
        /// <returns>True if the phone seems to be in airplane mode, else false.</returns>
        public static bool IsAirplaneMode
        {
            get
            {
                bool[] networks = new bool[4]
                { 
                    DeviceNetworkInformation.IsNetworkAvailable, 
                    DeviceNetworkInformation.IsCellularDataEnabled, 
                    DeviceNetworkInformation.IsCellularDataRoamingEnabled, 
                    DeviceNetworkInformation.IsWiFiEnabled 
                };

                return (networks.Count(n => n) < 1);
            }
        }

        /// <summary>
        /// Gets whether the phone has Wifi enabled.
        /// </summary>
        /// <returns>True if the phone has Wifi enabled, else false.</returns>
        public static bool HasWifi
        {
            get
            {
                return DeviceNetworkInformation.IsWiFiEnabled;
            }
        }

        /// <summary>
        /// Gets whether the phone has network connectivity.
        /// </summary>
        /// <returns>True if the phone has network connectivity, else false.</returns>
        public static bool HasNetwork
        {
            get
            {
                return DeviceNetworkInformation.IsNetworkAvailable;
            }
        }
    }
}
