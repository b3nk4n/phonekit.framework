/* 
    Copyright (c) 2012 - 2013 Microsoft Corporation.  All rights reserved.
    Use of this sample source code is subject to the terms of the Microsoft license 
    agreement under which you licensed this sample source code and is provided AS-IS.
    If you did not accept the terms of the license agreement, you are not authorized 
    to use this sample source code.  For the terms of the license, please see the 
    license agreement between you and Microsoft.
  
    To see all Code Samples for Windows Phone, visit http://code.msdn.microsoft.com/wpapps
  
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;


namespace Microsoft.Phone.Applications.Common
{
    public static class ApplicationSettingHelper
    {
        /// <summary>
        /// Attempt to get a value from the application settings
        /// if not successful (no present, wrong type), returns provided default value
        /// </summary>
        /// <typeparam name="TValue">Expected type of the setting</typeparam>
        /// <param name="key">Name of the key</param>
        /// <param name="defaultValue">Default value to be returned if fails</param>
        /// <returns></returns>
        public static TValue TryGetValueWithDefault<TValue>(string key, TValue defaultValue)
        {
            TValue retval = defaultValue;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
            {
                object value = IsolatedStorageSettings.ApplicationSettings[key];
                if (value is TValue)
                {
                    retval = (TValue) value;
                }
            }
            return retval;
        }


        /// <summary>
        /// Add key/value or Update existing key with new value to the application settings
        /// </summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">New or updated value</param>
        /// <returns></returns>
        public static bool AddOrUpdateValue(string key, Object value)
        {
            bool valueChanged = false;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
            {
                if (IsolatedStorageSettings.ApplicationSettings[key] != value)
                { // set to the value if it is different
                    IsolatedStorageSettings.ApplicationSettings[key] = value;
                    valueChanged = true;
                }
            }
            else
            { // key is not in dictionary, create it
                IsolatedStorageSettings.ApplicationSettings.Add(key, value);
                valueChanged = true;
            }
            return valueChanged;
        }


        /// <summary>
        /// Explicit Save. Not needed as Save will automatically be called at appInstance exit
        /// </summary>
        public static void Save()
        {
            IsolatedStorageSettings.ApplicationSettings.Save();
        }
    }
}
