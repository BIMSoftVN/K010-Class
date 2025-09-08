using K010Libs.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelWorkbook2.Classes
{
    public class clWeatherInfo : PropertyChangedBase
    {
        private DateTime? _wDate = null;
        public DateTime? wDate
        {
            get
            {
                return _wDate;
            }
            set
            {
                _wDate = value;
                OnPropertyChanged();
            }
        }


        private string _wMota = null;
        public string wMota
        {
            get
            {
                return _wMota;
            }
            set
            {
                _wMota = value;
                OnPropertyChanged();
            }
        }


        private double? _wTemp = null;
        public double? wTemp
        {
            get
            {
                return _wTemp;
            }
            set
            {
                _wTemp = value;
                OnPropertyChanged();
            }
        }

        private double? _wWind = null;
        public double? wWind
        {
            get
            {
                return _wWind;
            }
            set
            {
                _wWind = value;
                OnPropertyChanged();
            }
        }

        private double? _wRain = null;
        public double? wRain
        {
            get
            {
                return _wRain;
            }
            set
            {
                _wRain = value;
                OnPropertyChanged();
            }
        }

    }
}
