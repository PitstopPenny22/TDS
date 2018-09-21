using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Data
{
    /// <summary>
    /// A class that is responsible for managing data. This initialises the stream where to read data from.
    /// <para>Reads the stream, parses each line and processes it accordingly to create the data models.</para>
    /// </summary>
    public sealed class DataManager : IDataManager
    {
        private const string TyreDetailsFilePath = "Resources\\TyresXML.xml";
        private const string TrackDegradationCoefficientsFilePath = "Resources\\TrackDegradationCoefficients.txt";

        public List<TyreDetails> Tyres { get; private set; }
        public List<TrackDetails> Tracks { get; private set; }

        public async Task LoadAndParseData()
        {
            await LoadAndParseTyresDetails();
            await LoadAndParseTrackDetails();
        }

        private async Task LoadAndParseTyresDetails()
        {
            await Task.Run(() =>
            {
                using (var streamReader = OpenStreamReader(TyreDetailsFilePath))
                {
                    if (streamReader != null)
                    {
                        var tyresDetails = XElement.Load(streamReader);
                        Tyres = CreateTyreCollection(tyresDetails).ToList();
                        OnTyresDetailsPopulated();
                        streamReader.Close();
                    }
                    else
                    {
                        Debug.WriteLine("DataManager encountered a problem with StreamReader.");
                    }
                }
            });
        }
        private async Task LoadAndParseTrackDetails()
        {
            await Task.Run(() =>
            {
                using (var streamReader = OpenStreamReader(TrackDegradationCoefficientsFilePath))
                {
                    Tracks = CreateTrackDetailsCollection(streamReader).ToList();
                    OnTracksDetailsPopulated();
                    streamReader?.Close();
                }
            });
        }

        private IEnumerable<TyreDetails> CreateTyreCollection(XElement tyresDetails)
        {
            Debug.Assert(tyresDetails != null, "DataManager | CreateTyreCollection - tyresDetails cannot be null.");
            return tyresDetails.Elements("Tyre")
                       .Select(tyre => tyre.Elements().ToArray())
                       .Select(tyreDetails => new TyreDetails
                       {
                           Name = GetElementByName(tyreDetails, "Name"),
                           Family = GetElementByName(tyreDetails, "Family"),
                           Type = GetElementByName(tyreDetails, "Type"),
                           Placement = GetElementByName(tyreDetails, "Placement"),
                           DegradationCoefficient = double.Parse(GetElementByName(tyreDetails, "DegradationCoefficient"))
                       });
        }
        internal static string GetElementByName(IEnumerable<XElement> tyreDetails, string elementName)
        {
            Debug.Assert(!string.IsNullOrEmpty(elementName), "DataManager | GetElementByName - elementName cannot be null or empty string.");
            var childElementsByName = tyreDetails.FirstOrDefault(e => e.Name == elementName);
            if (childElementsByName != null)
            {
                return childElementsByName.Value;
            }
            Debug.WriteLine("DataManager error - cannot find element '{0}' in tyre details: '{1}'", elementName, tyreDetails.ToString());
            return null;
        }
   
        private IEnumerable<TrackDetails> CreateTrackDetailsCollection(StreamReader streamReader)
        {
            if (streamReader == null)
            {
                Debug.WriteLine("DataManager Error | CreateTrackDetailsCollection - streamReader cannot be null.");
                yield break;
            }
            Debug.Assert(streamReader != null);
            string trackDetails;
            while (streamReader.Peek() != -1)
            {
                trackDetails = streamReader.ReadLine();
                if (trackDetails == null)
                {
                    Debug.WriteLine("DataManager Error | CreateTrackDetailsCollection - cannot find details.");
                    yield break;
                }
                var separateDetails = trackDetails.Split('|');
                yield return new TrackDetails
                {
                    Name = separateDetails[0],
                    Location = separateDetails[1],
                    DegradationSamples = separateDetails[2].Split(',').ToArray()
                };
            }
        }

        /// <summary>
        /// Opens a <see cref="StreamReader"/> for the specified <paramref name="filePath"/>.
        /// </summary>
        private StreamReader OpenStreamReader(string filePath)
        {
            StreamReader streamReader = null;
            try
            {
                streamReader = new StreamReader(
                    new FileStream(
                        filePath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read)
                );
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("DataManager Error | OpenStreamReader Exception: {0}", ex.StackTrace);
                Debug.Fail("DataManager | Exception: " + ex.Message, ex.StackTrace);
            }
            return streamReader;
        }

        public event EventHandler TracksDetailsPopulated;
        private void OnTracksDetailsPopulated()
        {
            TracksDetailsPopulated?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler TyresDetailsPopulated;
        private void OnTyresDetailsPopulated()
        {
            TyresDetailsPopulated?.Invoke(this, EventArgs.Empty);
        }
    }
}