using System.IO.IsolatedStorage;

namespace PhoneKit.Framework.Core.Storage
{
    /// <summary>
    /// Encapsulates a key/value pair stored in isolated storage.
    /// </summary>
    /// <typeparam name="T">The type to store</typeparam>
    public class StoredObject<T>
    {
        #region Members

        /// <summary>
        /// The current value.
        /// </summary>
        private T _value;

        /// <summary>
        /// The default value.
        /// </summary>
        private T _defaultValue;

        /// <summary>
        /// The objects name.
        /// </summary>
        private string _name;

        /// <summary>
        /// Indicates whether a refresh from isolated storage is required.
        /// </summary>
        private bool _needRefresh;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new StoredObject instance.
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_defaultValue"></param>
        public StoredObject(string _name, T _defaultValue)
        {
            this._name = _name;
            this._defaultValue = _defaultValue;

            // if isolated storage doesn't have the value stored yet
            if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue(this._name, out this._value))
            {
                this._value = _defaultValue;
                IsolatedStorageSettings.ApplicationSettings[this._name] = _defaultValue;
            }

            this._needRefresh = false;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a string that represents the stored object.
        /// </summary>
        /// <returns>The stored objects string.</returns>
        public override string ToString()
        {
            return this._name
                + " with value: " + this._value.ToString()
                + ", default value: " + this._defaultValue.ToString();
        }

        /// <summary>
        /// Foreces a refresh from isolated storage.
        /// </summary>
        public void ForceRefresh()
        {
            this._needRefresh = true;
        }

        #endregion

        #region Private Metods

        #endregion

        #region Properties

        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value
        {
            get
            {
                if (this._needRefresh)
                {
                    // load value from isolated storage
                    if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue(this._name, out this._value))
                    {
                        IsolatedStorageSettings.ApplicationSettings[this._name] = this._defaultValue;
                        this._value = this._defaultValue;
                    }
                    this._needRefresh = false;
                }

                return this._value;
            }
            set
            {
                if (this._value != null && this._value.Equals(value))
                    return;

                // store the value in isolated storage
                IsolatedStorageSettings.ApplicationSettings[this._name] = value;
                this._value = value;
                this._needRefresh = true;
            }
        }

        /// <summary>
        /// Gets the default value.
        /// </summary>
        public T DefaultValue
        {
            get
            {
                return this._defaultValue;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
        }

        #endregion
    }
}
