using PhoneKit.Framework.Core.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneKit.TestApp.ViewModels
{
    /// <summary>
    /// The person list view model.
    /// </summary>
    class PersonListViewModel : ViewModelBase
    {
        /// <summary>
        /// The person list.
        /// </summary>
        private ObservableCollection<PersonViewModel> _personList = new ObservableCollection<PersonViewModel>();

        /// <summary>
        /// Indicates whether the data has been loaded.
        /// </summary>
        private bool _isLoaded = false;

        /// <summary>
        /// Loads the data.
        /// </summary>
        public void Load()
        {
            if (!_isLoaded)
            {
                // load sample data.
                _personList.Add(new PersonViewModel(
                    "Hans",
                    "Mustermann",
                    34));
                _personList.Add(new PersonViewModel(
                    "Karl",
                    "Mutter",
                    23));
                _personList.Add(new PersonViewModel(
                    "Hugo",
                    "Steinmeier",
                    54));
                _personList.Add(new PersonViewModel(
                    "Jürgen",
                    "Klose",
                    52));

                _isLoaded = true;
            }
        }

        /// <summary>
        /// Gets the person list.
        /// </summary>
        public ObservableCollection<PersonViewModel> PersonList
        {
            get
            {
                return _personList;
            }
        }
    }
}
