using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.CustomEventArgs;
using Data.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared;
using ViewModels;
using ViewModelUtils.Enums;

namespace ViewModelsTests
{
    [TestClass()]
    public class TyrePlacementViewModelTests
    {
        [ClassInitialize]
        public static void ClassSetup(TestContext context)
        {

            ConfigurationManager.AppSettings[AppUtils.AppSettings.OpenWeatherMapApiKey.ToString()] = "8e6138afab4aa2e9d5eb58fd8d590ade";
        }

        [TestMethod()]
        public void TyrePlacementViewModelTest_ValidPlacement()
        {
            var newTyrePlacement = new TyrePlacementViewModel(TyrePlacement.FL);
            Assert.AreNotEqual(null, newTyrePlacement);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void TyrePlacementViewModelTest_PlacementNotMappedToAxle()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new TyrePlacementViewModel(TyrePlacement.NotSet);
        }

    
        [TestMethod]
        public async Task CalculatePointTyreDegradationTest_Valid()
        {
            var dataManager = new DataManager();
            await dataManager.LoadAndParseData();
            var tyreDetailsViewModel = new TyreDetailsViewModel(dataManager.Tyres[0]);
        
            //e.g. Silverstone|Towcester|14,57
            const double trackDegradationPoint = 14;

            var tyreCoefficient = tyreDetailsViewModel.TyreCoefficient;
            double temperature = 0.0;
            var weatherService = new WeatherService(new OpenWeatherMapApiConsumer(new WebClientConsumer()));
            weatherService.GetTemperatureForLocationAsync("Towcester",
                (object sender, EventArgs e) =>
                {
                    if (e is TemperatureUpdatedEventArgs temperatureArgs)
                    {
                        temperature = temperatureArgs.NewTemperature;
                    }
                });
            
            var pointTyreDegradataion = TyrePlacementViewModel.CalculatePointTyreDegradation(trackDegradationPoint, tyreCoefficient, temperature);
            var manualCalculation = (14 + temperature) / tyreCoefficient;
            Assert.AreEqual(manualCalculation, pointTyreDegradataion);
        }

        [TestMethod]
        public async Task CalculatePointTyreDegradationTest_Invalid()
        {
            //e.g. Silverstone|Towcester|14,57
            const double trackDegradationPoint = 57;
            var dataManager = new DataManager();
            await dataManager.LoadAndParseData();
            var tyreDetailsViewModel = new TyreDetailsViewModel(dataManager.Tyres[0]);

            var tyreCoefficient = tyreDetailsViewModel.TyreCoefficient;
            double temperature = 0.0;
            var weatherService = new WeatherService(new OpenWeatherMapApiConsumer(new WebClientConsumer()));
            weatherService.GetTemperatureForLocationAsync("Towcester",
                (object sender, EventArgs e) =>
                {
                    if (e is TemperatureUpdatedEventArgs temperatureArgs)
                    {
                        temperature = temperatureArgs.NewTemperature;
                    }
                });

            var pointTyreDegradataion = TyrePlacementViewModel.CalculatePointTyreDegradation(trackDegradationPoint, tyreCoefficient, temperature);
            var manualCalculation = (14 + temperature) / tyreCoefficient;
            Assert.AreNotEqual(manualCalculation, pointTyreDegradataion);
        }

        [TestMethod]
        public async Task CalculateResults_Valid()
        {
            var dataManager = new DataManager();
            await dataManager.LoadAndParseData();
            var samplesList = dataManager.Tracks[0].GetDegradationSamples();
            var samplesListAsDouble = TrackDetailsViewModel.ConvertStringSamplesToDouble(samplesList);
            var newTyrePlacement =
                new TyrePlacementViewModel(TyrePlacement.FL)
                {
                    SelectedTyre = new TyreDetailsViewModel(dataManager.Tyres[0])
                };
            newTyrePlacement.CalculateResults(samplesListAsDouble, 22.28);
            Assert.AreEqual(8, newTyrePlacement.Average.Value);
            Assert.AreEqual(13, newTyrePlacement.Mode.Value);
            Assert.AreEqual(12, newTyrePlacement.Range.Value);
        }
        [TestMethod]
        public async Task CalculateResults_Invalid()
        {
            var dataManager = new DataManager();
            await dataManager.LoadAndParseData();
            var samplesList = dataManager.Tracks[0].GetDegradationSamples();
            var samplesListAsDouble = TrackDetailsViewModel.ConvertStringSamplesToDouble(samplesList);
            var newTyrePlacement =
                new TyrePlacementViewModel(TyrePlacement.FL)
                {
                    SelectedTyre = new TyreDetailsViewModel(dataManager.Tyres[0])
                };
            newTyrePlacement.CalculateResults(samplesListAsDouble, 22.28);
            Assert.AreNotEqual(9, newTyrePlacement.Average.Value);
            Assert.AreNotEqual(14, newTyrePlacement.Mode.Value);
            Assert.AreNotEqual(11, newTyrePlacement.Range.Value);
        }

        [TestMethod]
        public void CalculatePointTyreDegradation_Valid()
        {
            const int trackDegradationPoint = 14;
            const double tyreCoefficient = 0.8;
            const int temperature = 1;
            var point = TyrePlacementViewModel.CalculatePointTyreDegradation(trackDegradationPoint, tyreCoefficient, temperature);
            var manualCalculation = (trackDegradationPoint + temperature) / tyreCoefficient;
            Assert.AreEqual(manualCalculation, point);
        }

        [TestMethod]
        public void CalculatePointTyreDegradation_Invalid()
        {
            const int trackDegradationPoint = 14;
            const double tyreCoefficient = 0.8;
            const int temperature = 1;
            var point = TyrePlacementViewModel.CalculatePointTyreDegradation(trackDegradationPoint, tyreCoefficient, temperature);
            var manualWrongCalculation = (trackDegradationPoint + temperature) / 1.00;
            Assert.AreNotEqual(manualWrongCalculation, point);
        }
    }
}