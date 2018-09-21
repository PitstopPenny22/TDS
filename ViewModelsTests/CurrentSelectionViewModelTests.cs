using System;
using System.CodeDom;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Data;
using Data.CustomEventArgs;
using Data.Interfaces.Utils;
using Data.Utils;
using Moq;
using ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared;
using ViewModelUtils.Enums;
using ViewModelUtils.Interfaces;

namespace ViewModelsTests
{
    [TestClass]
    public class CurrentSelectionViewModelTests
    {
        private IHasTyresList<TyreDetailsViewModel> _mockedIHasListTyresDetails;

        [ClassInitialize]
        public static void ClassSetup(TestContext context)
        {
     
            ConfigurationManager.AppSettings[AppUtils.AppSettings.OpenWeatherMapApiKey.ToString()] = "8e6138afab4aa2e9d5eb58fd8d590ade";
        }

        [TestInitialize]
        public void TestSetup()
        {
            _mockedIHasListTyresDetails = new Mock<IHasTyresList<TyreDetailsViewModel>>(MockBehavior.Strict).Object;
        }

        [TestMethod()]
        public void GetTyrePlacementOnSameAxleAsTest_ValidPlacement()
        {
            var currentSelectionViewModel = new CurrentSelectionViewModel(_mockedIHasListTyresDetails, new WeatherService(new OpenWeatherMapApiConsumer(new WebClientConsumer())));
            var tyrePlacementOnSameAxleViewModel = currentSelectionViewModel.GetTyrePlacementOnSameAxleAs(currentSelectionViewModel.FrontLeft);
            Assert.IsTrue(tyrePlacementOnSameAxleViewModel != null);
            Assert.AreEqual(TyrePlacement.FR, tyrePlacementOnSameAxleViewModel.TyrePlacementTitle);
            Assert.AreNotEqual(TyrePlacement.RL, tyrePlacementOnSameAxleViewModel.TyrePlacementTitle);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void GetTyrePlacementOnSameAxleAsTest_InvalidPlacement()
        {
            // ReSharper disable once ObjectCreationAsStatement
             new TyrePlacementViewModel(TyrePlacement.NotSet);
        }

        [TestMethod()]
        public async Task UpdateTemperatureTest_SelectedTrack()
        {
            var dataManager = new DataManager();
            await dataManager.LoadAndParseData();
            var currentSelectionViewModel = new CurrentSelectionViewModel(_mockedIHasListTyresDetails, new WeatherService(new OpenWeatherMapApiConsumer(new WebClientConsumer())));
            var trackDetails = new Mock<TrackDetailsViewModel>(dataManager.Tracks[0]);
            currentSelectionViewModel.SelectedTrack = trackDetails.Object;
            currentSelectionViewModel.UpdateTemperature();
            double temperature = 0.0;
            var weatherService = new WeatherService(new OpenWeatherMapApiConsumer(new WebClientConsumer()));
            weatherService.GetTemperatureForLocationAsync(currentSelectionViewModel.SelectedTrack.Location, 
                (object sender, EventArgs e) =>
                {
                    if (e is TemperatureUpdatedEventArgs temperatureArgs)
                    {
                        temperature = temperatureArgs.NewTemperature;
                    }
                });
            Assert.AreEqual(temperature, currentSelectionViewModel.SelectedTrackTemperature);
        }

        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateTemperatureTest_NoSelectedTrack()
        {
            var currentSelectionViewModel = new CurrentSelectionViewModel(_mockedIHasListTyresDetails, new WeatherService(new OpenWeatherMapApiConsumer(new WebClientConsumer())));
            currentSelectionViewModel.UpdateTemperature();
        }

        [TestMethod]
        public void GetTyrePlacementOnSameAxleAsTest()
        {
            var currentSelectionViewModel = new CurrentSelectionViewModel(_mockedIHasListTyresDetails, new WeatherService(new OpenWeatherMapApiConsumer(new WebClientConsumer())));
            var tyrePlacement = currentSelectionViewModel.GetTyrePlacementOnSameAxleAs(currentSelectionViewModel.FrontLeft);
            Assert.AreEqual(currentSelectionViewModel.FrontRight, tyrePlacement);
        }

        [TestMethod]
        public async Task IsNewTyreOnSameAxleSameFamilyTest_SameFamily()
        {
            var dataManager = new DataManager();
            await dataManager.LoadAndParseData();
            var currentSelectionViewModel = new CurrentSelectionViewModel(_mockedIHasListTyresDetails, new WeatherService(new OpenWeatherMapApiConsumer(new WebClientConsumer())));
            currentSelectionViewModel.FrontLeft.SelectedTyre = new TyreDetailsViewModel(dataManager.Tyres.First(t => t.Family == "F1" && t.Placement == "FL"));
            currentSelectionViewModel.FrontRight.SelectedTyre = new TyreDetailsViewModel(dataManager.Tyres.First(t => t.Family == "F1" && t.Placement == "FR"));
            var isTyreOnSameAxleSameFamily = currentSelectionViewModel.IsNewTyreOnSameAxleSameFamily(currentSelectionViewModel.FrontLeft);
            Assert.IsTrue(isTyreOnSameAxleSameFamily);
        }
        [TestMethod]
        public async Task IsNewTyreOnSameAxleSameFamilyTest_DifferentFamily()
        {
            var dataManager = new DataManager();
            await dataManager.LoadAndParseData();
            var currentSelectionViewModel = new CurrentSelectionViewModel(_mockedIHasListTyresDetails, new WeatherService(new OpenWeatherMapApiConsumer(new WebClientConsumer())));
            currentSelectionViewModel.FrontLeft.SelectedTyre = new TyreDetailsViewModel(dataManager.Tyres.First(a => a.Family == "F1" && a.Placement == "FL"));
            currentSelectionViewModel.FrontRight.SelectedTyre = new TyreDetailsViewModel(dataManager.Tyres.First(a => a.Family == "F2" && a.Placement == "FR"));
            // Changing the family for FrontRight, should void the selection for FrontLeft since it's on the same Axle. This is one of the validation rules.
            Assert.IsTrue(currentSelectionViewModel.FrontLeft.SelectedTyre == null);
            Assert.IsFalse(currentSelectionViewModel.FrontRight.SelectedTyre == null);
        }

        [TestMethod]
        public async Task IsNewTyreSameTypeAsOthers_SameType()
        {
            var dataManager = new DataManager();
            await dataManager.LoadAndParseData();
            var currentSelectionViewModel = new CurrentSelectionViewModel(_mockedIHasListTyresDetails, new WeatherService(new OpenWeatherMapApiConsumer(new WebClientConsumer())));
            currentSelectionViewModel.FrontLeft.SelectedTyre = new TyreDetailsViewModel(dataManager.Tyres.First(t => t.Type == "SuperSoft" && t.Placement == "FL"));
            currentSelectionViewModel.FrontRight.SelectedTyre = new TyreDetailsViewModel(dataManager.Tyres.First(t => t.Type == "SuperSoft" && t.Placement == "FR"));
            currentSelectionViewModel.RearLeft.SelectedTyre = new TyreDetailsViewModel(dataManager.Tyres.First(t => t.Type == "SuperSoft" && t.Placement == "RL"));
            currentSelectionViewModel.RearRight.SelectedTyre = new TyreDetailsViewModel(dataManager.Tyres.First(t => t.Type == "SuperSoft" && t.Placement == "RR"));

            var isTyreSameTypeAsOthers = currentSelectionViewModel.IsNewTyreSameTypeAsOthers(currentSelectionViewModel.FrontLeft);
            Assert.IsTrue(isTyreSameTypeAsOthers);
        }

        [TestMethod]
        public async Task IsNewTyreSameTypeAsOthers_DifferentType()
        {
            var dataManager = new DataManager();
            await dataManager.LoadAndParseData();

            var currentSelectionViewModel = new CurrentSelectionViewModel(_mockedIHasListTyresDetails, new WeatherService(new OpenWeatherMapApiConsumer(new WebClientConsumer())));
            currentSelectionViewModel.FrontLeft.SelectedTyre = new TyreDetailsViewModel(dataManager.Tyres.First(t => t.Type == "SuperSoft" && t.Placement == "FL"));
            currentSelectionViewModel.FrontRight.SelectedTyre = new TyreDetailsViewModel(dataManager.Tyres.First(t => t.Type == "SuperSoft" && t.Placement == "FR"));
            currentSelectionViewModel.RearLeft.SelectedTyre = new TyreDetailsViewModel(dataManager.Tyres.First(t => t.Type == "SuperSoft" && t.Placement == "RL"));
            currentSelectionViewModel.RearRight.SelectedTyre = new TyreDetailsViewModel(dataManager.Tyres.First(t => t.Type == "Medium" && t.Placement == "RR"));
            //Changing RR to be Medium, should void the others. This is one of the validation rules.
            Assert.IsTrue(currentSelectionViewModel.FrontLeft.SelectedTyre == null);
            Assert.IsTrue(currentSelectionViewModel.FrontRight.SelectedTyre == null);
            Assert.IsTrue(currentSelectionViewModel.RearLeft.SelectedTyre == null);
            Assert.IsFalse(currentSelectionViewModel.RearRight.SelectedTyre == null);
        }
    }
}