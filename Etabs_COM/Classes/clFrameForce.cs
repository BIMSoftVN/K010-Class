using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etabs_COM.Classes
{
    public class clFrameForce
    {
        public string FrameName { get; set; }
        public string Label { get; set; }
        public string Story { get; set; }
        public string LoadCase { get; set; }
        public double? P { get; set; }
        public double? M2 { get; set; }
        public double? M3 { get; set; }
        
    }
}
