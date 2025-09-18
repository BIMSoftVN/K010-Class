using DevExpress.Mvvm;
using DevExpress.Xpf.Spreadsheet;
using K010Libs.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ExcelWorkbook2.ViewModels
{
    public class vmExcelPane : PropertyChangedBase
    {
        private string _Email = null;
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
                OnPropertyChanged();
            }
        }

        private DelegateCommand showText_Cmd;
        public ICommand ShowText_Cmd
        {
            get
            {
                if (showText_Cmd == null)
                {
                    showText_Cmd = new DelegateCommand(PerformShowText_Cmd);
                }

                return showText_Cmd;
            }
        }

        private void PerformShowText_Cmd()
        {
            MessageBox.Show("Đang nhập " + Email, "Thông báo");
        }

    }
}
