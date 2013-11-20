using Microsoft.Phone.Shell;

namespace PhoneKit.Framework.Storage
{
    public static class PhoneStateHelper
    {
        /// <summary>
        /// Saves a value in the phone application state.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value to store.</param>
        /// <returns>Returns whether the value has been saved.</returns>
        public static bool SaveValue(string key, object value)
        {
            // validate data
            if (value == null)
                return false;

            var store = PhoneApplicationService.Current.State;
            if (store.ContainsKey(key))
                store[key] = value;
            else
                store.Add(key, value);

            return true;
        }

        /// <summary>
        /// Loads a value from phone application state.
        /// </summary>
        /// <typeparam name="T">The type to load.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The loaded value or the types default value.</returns>
        public static T LoadValue<T>(string key)
        {
            return LoadValue<T>(key, default(T));
        }

        /// <summary>
        /// Loads a value from phone application state.
        /// </summary>
        /// <typeparam name="T">The type to load.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value, if no value was stored.</param>
        /// <returns>Returns the loaded value or the default value.</returns>
        public static T LoadValue<T>(string key, T defaultValue)
        {
            var store = PhoneApplicationService.Current.State;
            if (!store.ContainsKey(key))
                return defaultValue;

            return (T)store[key];
        }

        /// <summary>
        /// Verifies whether the an value in the phone application state.
        /// for the given key exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns true if the key exists, else false.</returns>
        public static bool ValueExists(string key)
        {
            var store = PhoneApplicationService.Current.State;
            return store.ContainsKey(key);
        }

        /// <summary>
        /// Deletes a value from phone application state.
        /// </summary>
        /// <param name="key">The key.</param>
        public static void DeleteValue(string key)
        {
            var store = PhoneApplicationService.Current.State;
            if (store.ContainsKey(key))
                store.Remove(key);
        }
    }
}
