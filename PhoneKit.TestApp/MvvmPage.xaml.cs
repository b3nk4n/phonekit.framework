using Microsoft.Phone.Controls;
using PhoneKit.TestApp.ViewModels;

namespace PhoneKit.TestApp
{
    public partial class MvvmPage : PhoneApplicationPage
    {
        public MvvmPage()
        {
            InitializeComponent();

            var persons = new PersonListViewModel();
            persons.Load();

            this.DataContext = persons;
        }
    }
}