using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etabs_COM.Classes
{
    public class clArea
    {
        public string Name { get; set; }
        public double[] X { get; set; } = new double[] {};
        public double[] Y { get; set; } = new double[] { };
        public double[] Z { get; set; } = new double[] { };
    }
}
