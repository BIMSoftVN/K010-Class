using K010Libs.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelWorkbook2.Classes
{
    public  class clText : PropertyChangedBase
    {
        private string _TextString = null;
        public string TextString
        {
            get
            {
                return _TextString;
            }
            set
            {
                _TextString = value;
                OnPropertyChanged();
            }
        }

        private double[] _Orgin = new double[3];
        public double[] Orgin
        {
            get
            {
                return _Orgin;
            }
            set
            {
                _Orgin = value;
                OnPropertyChanged();
            }
        }

    }
}
