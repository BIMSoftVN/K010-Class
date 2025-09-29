using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using K010Libs.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCAD_NET.TKC
{
    public class clText : PropertyChangedBase
    {
        private ObjectId? _Id = null;
        public ObjectId? Id
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

        private Point3d _Orgin = new Point3d();
        public Point3d Orgin
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
