using Autodesk.Revit.DB;
using K010Libs.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K010_RevitAddins.ImportCAD
{
    public class vmMain : PropertyChangedBase
    {
        private ImportInstance _LinkCAD = null;

        public ImportInstance LinkCAD
        {
            get 
            { 
                return _LinkCAD; 
            }
            set 
            { 
                _LinkCAD = value; 
                OnPropertyChanged(); 
            }
        }

    }
}
