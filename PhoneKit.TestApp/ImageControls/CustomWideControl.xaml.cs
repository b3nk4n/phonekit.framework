using System.Windows.Controls;
using System.Windows.Media;

namespace PhoneKit.TestApp.ImageControls
{
    public partial class CustomWideControl : UserControl
    {
        public CustomWideControl(Color color)
        {
            InitializeComponent();

            LayoutRoot.Background = new SolidColorBrush(color);
        }
    }
}
