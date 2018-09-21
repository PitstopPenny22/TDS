using System;
using System.Collections.ObjectModel;

namespace Data
{
    /// <summary>
    /// Struct that groups together details for a track as gathered from reading the .txt file.
    /// </summary>
    public struct TrackDetails
    {
        public string Name { get; internal set; }
        public string Location { get; internal set; }
        internal string[] DegradationSamples;

        public ReadOnlyCollection<string> GetDegradationSamples()
        {
            return Array.AsReadOnly(DegradationSamples);
        }
    }
}