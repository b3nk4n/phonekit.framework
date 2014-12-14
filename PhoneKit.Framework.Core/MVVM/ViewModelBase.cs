using System.ComponentModel;
using System.Runtime.Serialization;

namespace PhoneKit.Framework.Core.MVVM
{
    /// <summary>
    /// The base class for every view model of the MVVM pattern.
    /// </summary>
    [DataContract]
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// The property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates a ViewModelBaseInstance.
        /// </summary>
        /// <remarks>Required for deserialization.</remarks>
        public ViewModelBase() { }

        /// <summary>
        /// Notifies the binding system that the specified property was changed.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
    }
}
