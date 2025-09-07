using K010Libs.Mvvm;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ExcelAddIn1
{
    public class vmTest : PropertyChangedBase
    {
        private string _TextValue = "Test";
        public string TextValue
        {
            get
            {
                return _TextValue;
            }
            set
            {
                _TextValue = value;
                OnPropertyChanged();
            }
        }



        private ActionCommand btnCmnd1;
        public ICommand btnCmnd
        {
            get
            {
                if (btnCmnd1 == null)
                {
                    btnCmnd1 = new ActionCommand(PerformbtnCmnd);
                }

                return btnCmnd1;
            }
        }

        private void PerformbtnCmnd()
        {
            MessageBox.Show(TextValue);
        }
    }
}
