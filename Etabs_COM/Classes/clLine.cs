using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etabs_COM.Classes
{
    public class clLine
    {
        public string Name { get; set; }

        public clPoint StartPoint { get; set; } = new clPoint();

        public clPoint EndPoint { get; set; } = new clPoint();
       
    }

    public class clPoint
    {
        public string Name { get; set; }
        public double? X { get; set; }
        public double? Y { get; set; }
        public double? Z { get; set; }
    }
}
