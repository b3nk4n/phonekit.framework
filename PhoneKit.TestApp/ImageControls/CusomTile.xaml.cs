using System.Windows.Controls;
using System.Windows.Media;

namespace PhoneKit.TestApp.ImageControls
{
    public partial class CusomTile : UserControl
    {
        public CusomTile(Color color)
        {
            InitializeComponent();

            LayoutRoot.Background = new SolidColorBrush(color);
        }
    }
}
