using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
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