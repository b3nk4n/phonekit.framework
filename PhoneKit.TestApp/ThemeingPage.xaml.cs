using System.Windows;
using Microsoft.Phone.Controls;
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