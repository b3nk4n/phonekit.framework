using PhoneKit.Framework.Core.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhoneKit.TestApp.ViewModels
{
    /// <summary>
    /// The person view model.
    /// </summary>
    public class PersonViewModel : ViewModelBase
    {
        /// <summary>
        /// The first name.
        /// </summary>
        private string _firstName;

        /// <summary>
        /// The last name.
        /// </summary>
        private string _lastName;

        /// <summary>
        /// The age.
        /// </summary>
        private int _age;

        private ICommand _happyBirthdayCommand;

        /// <summary>
        /// Creates a PersonViewModel instance.
        /// </summary>
        /// <param name="first">The first name.</param>
        /// <param name="last">The last name.</param>
        /// <param name="age">The age.</param>
        public PersonViewModel(string first, string last, int age)
        {
            FirstName = first;
            LastName = last;
            Age = age;
            _happyBirthdayCommand = new DelegateCommand(
            () => {
                this.Age += 1;
            },
            () => {
                return this.Age < 100;
            });
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    NotifyPropertyChanged("FirstName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    NotifyPropertyChanged("LastName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                if (_age != value)
                {
                    _age = value;
                    NotifyPropertyChanged("Age");
                }
            }
        }

        public ICommand HappyBirthdayCommand
        {
            get
            {
                return _happyBirthdayCommand;
            }
        }
    }
}
