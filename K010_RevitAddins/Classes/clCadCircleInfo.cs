using Autodesk.Revit.DB;
using K010Libs.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K010_RevitAddins.Classes
{
    public class clCadCircleInfo : PropertyChangedBase
    {
        private double? _X = null;
        public double? X
        {
            get
            {
                return _X;
            }
            set
            {
                _X = value;
                OnPropertyChanged();
            }
        }

        private double? _Y = null;
        public double? Y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
                OnPropertyChanged();
            }
        }

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


        private string _Layer = null;
        public string Layer
        {
            get
            {
                return _Layer;
            }
            set
            {
                _Layer = value;
                OnPropertyChanged();
            }
        }

        private string _TypeName = null;
        public string TypeName
        {
            get
            {
                return _TypeName;
            }
            set
            {
                _TypeName = value;
                OnPropertyChanged();
            }
        }

        private ElementId _TypeId;
        public ElementId TypeId
        {
            get
            {
                return _TypeId;
            }
            set
            {
                _TypeId = value;
                OnPropertyChanged();
            }
        }
    }
}
