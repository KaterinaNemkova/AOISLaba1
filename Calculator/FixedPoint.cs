using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba1AOIS
{
    public class FixedPoint
    {
        public string IntegerPart { get; set; }
        public string FractionalPart { get; set; }

        public FixedPoint(string integerPart, string fractionalPart)
        {
            IntegerPart = integerPart;
            FractionalPart = fractionalPart;
        }
    }
}
