using Autodesk.Revit.DB;
using K010Libs.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K010_RevitAddins.Classes
{
    public class clFamilyCoc : PropertyChangedBase
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


        private string _FamilyName = null;
        public string FamilyName
        {
            get
            {
                return _FamilyName;
            }
            set
            {
                _FamilyName = value;
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

        private ElementId _Id = null;
        public ElementId Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
                OnPropertyChanged();
            }
        }


    }
}
