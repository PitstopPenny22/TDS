using System;
using Data;
using ViewModelUtils.Enums;

namespace ViewModels
{
    /// <summary>
    /// ViewModel to represent a tyre and its respective details.
    /// </summary>
    public class TyreDetailsViewModel : ViewModelBase
    {
        private string _name;

        /// <summary>
        /// Display name of the tyre. 
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            private set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Family group of the tyre e.g. F1
        /// </summary>
        internal TyreFamily Family { get; }

        /// <summary>
        /// Tyre type e.g. Supersoft 
        /// </summary>
        public TyreType Type { get; }
       
        /// <summary>
        /// Where the tyre can be placed e.g. Front Left [FL]
        /// </summary>
        public TyrePlacement Placement { get; }

        internal double TyreCoefficient { get; }

        public TyreDetailsViewModel(TyreDetails tyre)
        {
            Name = tyre.Name;
            Family = tyre.Family.ToEnum<TyreFamily>();
            Placement = tyre.Placement.ToEnum<TyrePlacement>();
            Type = tyre.Type.ToEnum<TyreType>();
            var degradationCoefficient = tyre.DegradationCoefficient;
            TyreCoefficient = CalculateTyreCoefficient(GetPercentageValue(), degradationCoefficient);
        }

        public bool IsSameFamilyAs(TyreDetailsViewModel anotherTyre)
        {
            return anotherTyre?.Family == Family;
        }

        internal static double CalculateTyreCoefficient(double percentage, double degradationCoefficient)
        {
            return (percentage / 100.00) * degradationCoefficient; // Adding .00 to 100.00 so the result is a double
        }

        /// <summary>
        /// Gets the percentage value associated with the type of this tyre. This is obtained via the <see cref="PercentageAttribute"/>
        /// <para>set on <see cref="TyreType"/>.</para>
        /// <para>If no <see cref="PercentageAttribute"/> is set, then an exception is thrown.</para>
        /// </summary>
        internal double GetPercentageValue()
        {
            var percentages = Type.GetAttributeValues<PercentageAttribute>(typeof(TyreType));
            if (percentages.Length > 0)
            {
                return percentages[0].PercentageValue;
            }
            throw new Exception($"TyreDetailsViewModel GetPercentageValue Error | {Type} has not been given a percentage attribute setting.");
        }
    }
}