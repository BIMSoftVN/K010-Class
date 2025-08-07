using DevExpress.Mvvm;
using DevExpress.Xpf.WindowsUI;
using iCons.Views;
using K010Libs.Mvvm;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace iCons.ViewModels
{
    public class vmMain : PropertyChangedBase
    {





        private ActionCommand openAccInfoWin_Command;

        public ICommand OpenAccInfoWin_Command
        {
            get
            {
                if (openAccInfoWin_Command == null)
                {
                    openAccInfoWin_Command = new ActionCommand(OpenAccInfoWin_);
                }

                return openAccInfoWin_Command;
            }
        }

        private void OpenAccInfoWin_(object par)
        {
            var win = par as WinUIDialogService;
            if (win != null)
            {
                var kq = win.ShowDialog(dialogButtons: MessageButton.OKCancel, title:"Thông tin người dùng", viewModel: this);

                if (kq == MessageResult.OK)
                {

                }    
            }
        }
    }
}
