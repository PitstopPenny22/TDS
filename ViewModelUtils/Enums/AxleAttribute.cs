using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelUtils.Enums
{
    public class AxleAttribute : Attribute
    {
        public Axle Axle { get; }
        internal AxleAttribute(Axle axle)
        {
            Axle = axle;
        }
    }
}