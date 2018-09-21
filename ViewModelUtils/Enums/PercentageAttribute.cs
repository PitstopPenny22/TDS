using System;

namespace ViewModelUtils.Enums
{
    public class PercentageAttribute : Attribute
    {
        public double PercentageValue { get; }
        internal PercentageAttribute(double percentageValue)
        {
            PercentageValue = percentageValue;
        }
    }
}
