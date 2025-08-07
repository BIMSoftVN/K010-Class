using DevExpress.Xpf.WindowsUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace iCons.Views
{
    /// <summary>
    /// Interaction logic for vMain.xaml
    /// </summary>
    public partial class vMain : Window
    {
        public vMain()
        {
            InitializeComponent();
            var winSignIn = new vSignIn();
            winSignIn.ShowDialog();
        }

        private void HamburgerMenuBottomBarNavigationButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var btn = sender as HamburgerMenuBottomBarNavigationButton;
            if (btn!=null && btn.ContextMenu!=null)
            {
                btn.ContextMenu.PlacementTarget = btn;
                btn.ContextMenu.IsOpen = true;
            }    
        }
    }
}
