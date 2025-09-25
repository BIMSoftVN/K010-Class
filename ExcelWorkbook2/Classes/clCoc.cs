using AutoCAD;
using K010Libs.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelWorkbook2.Classes
{
    public class clCoc : PropertyChangedBase
    {
        private double? _Diameter = null;
        public double? Diameter
        {
            get
            {
                return _Diameter;
            }
            set
            {
                _Diameter = value;
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

        private string _Name = null;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }
    }
}
