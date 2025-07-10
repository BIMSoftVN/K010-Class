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
using TaskManage.ViewModels;

namespace TaskManage.Views
{
    /// <summary>
    /// Interaction logic for vSignIn.xaml
    /// </summary>
    public partial class vSignIn : Window
    {
        public vSignIn()
        {
            InitializeComponent();
            (this.DataContext as vmSignIn).win = this;
        }

        private void CloseWin(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();  
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
