using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneKit.Framework.Storage;

namespace PhoneKit.TestApp
{
    public partial class Settings : PhoneApplicationPage
    {
        public static StoredObject<bool> ToggleValue = new StoredObject<bool>("ToggleValue", true);
        public static StoredObject<bool> CheckBoxValue = new StoredObject<bool>("CheckBoxValue", true);

        public Settings()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.TestToggleSwitch.IsChecked = ToggleValue.Value;
            this.TextCheckBox.IsChecked = CheckBoxValue.Value;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            ToggleValue.Value = this.TestToggleSwitch.IsChecked.Value;
            CheckBoxValue.Value = this.TextCheckBox.IsChecked.Value;
        }
    }
}