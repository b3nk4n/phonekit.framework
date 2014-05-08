using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneKit.Framework.Core.Themeing;
using System.Windows.Media;

namespace PhoneKit.TestApp
{
    public partial class ThemeingPage : PhoneApplicationPage
    {
        public ThemeingPage()
        {
            InitializeComponent();
            TextBlockActiveTheme.Text = PhoneThemeHelper.IsDarkThemeActive == true ? "DARK" : "LIGHT";

            var beforeColor = (Color)Application.Current.Resources["PhoneAccentColor"];
            var beforeBrush = (SolidColorBrush)Application.Current.Resources["PhoneAccentBrush"];
        }
    }
}