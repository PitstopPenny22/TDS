using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Data;

namespace ViewModels
{
    /// <summary>
    /// ViewModel that represents a track and its respective details.
    /// </summary>
    public class TrackDetailsViewModel : ViewModelBase
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Location { get; }
        public ReadOnlyCollection<double> Samples { get; }

        public TrackDetailsViewModel(TrackDetails details)
        {
            Name = details.Name;
            Location = details.Location;
            Samples = ConvertStringSamplesToDouble(details.GetDegradationSamples());
        }

        internal static ReadOnlyCollection<double> ConvertStringSamplesToDouble(IReadOnlyList<string> degradationSamples)
        {
            var samplesAsDouble = new double[degradationSamples.Count];
            bool success;
            for (var i = 0; i < degradationSamples.Count; i++)
            {
                success = double.TryParse(degradationSamples[i].Trim(), out samplesAsDouble[i]);
                if (!success)
                {
                    throw new Exception("TrackDetailsViewModel Error | Incorrect type of sample data. Expecting int.");
                }
            }
            return new ReadOnlyCollection<double>(samplesAsDouble);
        }
    }
}