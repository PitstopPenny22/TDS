using Autofac;
using Data;
using Data.Interfaces;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Data.Interfaces.Utils;
using Data.Utils;
using ViewModelUtils.Interfaces;

namespace ViewModels
{
    /// <summary>
    /// The Main ViewModel for the application. This creates a ViewModel representation of
    /// <para>the models to be used and interacted with by the Views.</para>
    /// </summary>
    public sealed class MainViewModel : ViewModelBase, IHasTyresList<TyreDetailsViewModel>
    {
        public static IContainer Container { get; private set; }

        private ObservableCollection<TrackDetailsViewModel> _tracksDetailsCollection;

        public RelayCommand LoadDataCommand { get; }
        public List<TyreDetailsViewModel> TyresList { get; private set; }
        public ObservableCollection<TrackDetailsViewModel> TracksDetailsCollection
        {
            get { return _tracksDetailsCollection; }
            private set
            {
                _tracksDetailsCollection = value; 
                OnPropertyChanged("TracksDetailsCollection");
            }
        }
        public CurrentSelectionViewModel CurrentSelectionViewModel { get; }

        public MainViewModel()
        {
            InitContainer();
            LoadDataCommand = new RelayCommand(async () => await LoadData());
           
            CurrentSelectionViewModel = new CurrentSelectionViewModel(this, Container.Resolve<IWeatherService>());
        }

        private async Task LoadData()
        {
            var dataManager = Container.Resolve<IDataManager>();
            dataManager.TracksDetailsPopulated += Current_TracksDetailsPopulated;
            dataManager.TyresDetailsPopulated += Current_TyresDetailsPopulated;

            await dataManager.LoadAndParseData();
        }

        private void InitContainer()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<DataManager>().As<IDataManager>();
            containerBuilder.RegisterType<WebClientConsumer>().As<IWebClientConsumer>();
            containerBuilder.RegisterType<OpenWeatherMapApiConsumer>().As<IOpenWeatherMapApiConsumer>();
            containerBuilder.RegisterType<WeatherService>().As<IWeatherService>();

            Container = containerBuilder.Build();
        }

        private void Current_TyresDetailsPopulated(object sender, EventArgs e)
        {
            Debug.WriteLine("MainViewModel | Current_TyresDetailsPopulated");
            if (sender is DataManager dataManager)
            {
                dataManager.TyresDetailsPopulated -= Current_TyresDetailsPopulated;
                UpdateTyresCollection(dataManager.Tyres);
            }
        }
        private void Current_TracksDetailsPopulated(object sender, EventArgs e)
        {
            Debug.WriteLine("MainViewModel | Current_TracksDetailsPopulated");
            if (sender is DataManager dataManager)
            {
                dataManager.TracksDetailsPopulated -= Current_TracksDetailsPopulated;
                UpdateTracksCollection(dataManager.Tracks);
            }
        }
        private void UpdateTyresCollection(List<TyreDetails> currentTyres)
        {
            if (currentTyres == null)
            {
                // If currentTyres is still null, then set TyresDetailsCollection to null;
                TyresList = null;
                return;
            }
            TyresList = new List<TyreDetailsViewModel>();
            foreach (var tyre in currentTyres)
            {
                TyresList.Add(new TyreDetailsViewModel(tyre));
            }
            OnTyresDetailsListUpdated();
        }
        private void UpdateTracksCollection(List<TrackDetails> currentTracks)
        {
            UpdateOnDispatcherThread(() =>
            {
                if (currentTracks == null)
                {
                    // If currentTracks is still null, then set TracksDetailsCollection to null
                    TracksDetailsCollection = null;
                    return;
                }

                TracksDetailsCollection = new ObservableCollection<TrackDetailsViewModel>();
                foreach (var track in currentTracks)
                {
                    TracksDetailsCollection.Add(new TrackDetailsViewModel(track));
                }
                OnPropertyChanged("TracksDetailsCollection");
            });
        }

        public event EventHandler TyresListUpdated;
        private void OnTyresDetailsListUpdated()
        {
            TyresListUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}