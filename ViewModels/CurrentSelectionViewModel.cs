using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Data.CustomEventArgs;
using Data.Interfaces.Utils;
using Data.Utils;
using ViewModelUtils.Enums;
using ViewModelUtils.Interfaces;

namespace ViewModels
{
    /// <summary>
    /// ViewModel that represents the current selection of tyres for available tyre placements (e.g. FL, FR, etc.) and 
    /// <para>selected track with corresponding temperature.</para>
    /// </summary>
    public sealed class CurrentSelectionViewModel : ViewModelBase
    {
        private TrackDetailsViewModel _selectedTrack;
        private double _selectedTrackTemperature;
        private readonly ReaderWriterLockSlim _readerWriterLockSlim = new ReaderWriterLockSlim();
        private IWeatherService _weatherService;

        public TyrePlacementViewModel FrontLeft { get; }
        public TyrePlacementViewModel FrontRight { get; }
        public TyrePlacementViewModel RearLeft { get; }
        public TyrePlacementViewModel RearRight { get; }
        public TrackDetailsViewModel SelectedTrack
        {
            get
            {
                _readerWriterLockSlim.EnterReadLock();
                var selectedTrack = _selectedTrack;
                _readerWriterLockSlim.ExitReadLock();
                return selectedTrack;
            }
            set
            {
                _readerWriterLockSlim.EnterWriteLock();
                _selectedTrack = value;
                _readerWriterLockSlim.ExitWriteLock();
                OnPropertyChanged("SelectedTrack");
            }
        }
        public double SelectedTrackTemperature
        {
            get
            {
                _readerWriterLockSlim.EnterReadLock();
                var selectedTrackTemperature = _selectedTrackTemperature;
                _readerWriterLockSlim.ExitReadLock();
                return selectedTrackTemperature;
            }
            set
            {
                _readerWriterLockSlim.EnterWriteLock();
                _selectedTrackTemperature = value;
                _readerWriterLockSlim.ExitWriteLock();
                OnPropertyChanged("SelectedTrackTemperature");
                UpdateResults();
            }
        }

        public TyrePlacementViewModel[] AllTyrePlacements { get; }

        public CurrentSelectionViewModel(IHasTyresList<TyreDetailsViewModel> hasTyresDetailsList, IWeatherService weatherService)
        {
            hasTyresDetailsList.TyresListUpdated += TyresListUpdated_UpdateAllowedTyres;
            _weatherService = weatherService;

            FrontLeft = new TyrePlacementViewModel(TyrePlacement.FL);
            FrontRight = new TyrePlacementViewModel(TyrePlacement.FR);
            RearLeft = new TyrePlacementViewModel(TyrePlacement.RL);
            RearRight = new TyrePlacementViewModel(TyrePlacement.RR);
            AllTyrePlacements = new[] { FrontLeft, FrontRight, RearLeft, RearRight };
            foreach (var tyrePlacement in AllTyrePlacements)
            {
                tyrePlacement.SelectedTyreChanged += TyrePlacement_SelectedTyreChanged;
            }
        }

        /// <summary>
        /// When the MainViewModel's list of tyres gets updated, update the list of tyres allowed
        /// <para>on each placement.</para>
        /// </summary>
        private void TyresListUpdated_UpdateAllowedTyres(object sender, EventArgs e)
        {
            var mainViewModel = sender as MainViewModel;
            if (mainViewModel == null)
            {
                return;
            }
            foreach (var tyre in AllTyrePlacements)
            {
                tyre.UpdateAllowedTyresCollection(mainViewModel.TyresList);
            }
        }

        private void TyrePlacement_SelectedTyreChanged(object sender, EventArgs e)
        {
            var tyrePlacementWithNewSelectedTyre = sender as TyrePlacementViewModel;
            if (tyrePlacementWithNewSelectedTyre?.SelectedTyre == null)
            {
                return;
            }
            HandleNewTyreSelection(tyrePlacementWithNewSelectedTyre);
        }

        private void HandleNewTyreSelection(TyrePlacementViewModel tyrePlacementWithNewSelectedTyre)
        {
            if (tyrePlacementWithNewSelectedTyre.SelectedTyre == null)
            {
                // Nothing to validate
                return;
            }

            /*  Validation rules are as follows:
                •	All four tyres should be of the same type (i.e. SuperSoft)
                •	Only tyres of the same Family should be placed on the same axle (e.g. if Front Left tyre Family = ‘F1’ then Front Right tyre must also have Family = ‘F1’.
                    Rear tyres must be of the same Family but this can be different to the Front tyres, for example, ‘F5’)
                •	Incompatible tyre selection(s) should be unselected when a new selection is made that breaks either of the above rules.
            */
            var doesNewSelectionMatchFamilyOnAxle = IsNewTyreOnSameAxleSameFamily(tyrePlacementWithNewSelectedTyre);
            if (!doesNewSelectionMatchFamilyOnAxle)
            {
                ClearIncompatibleTyreFamilyOnSameAxle(tyrePlacementWithNewSelectedTyre);
            }
            var isNewSelectionSameTypeAsOthers = IsNewTyreSameTypeAsOthers(tyrePlacementWithNewSelectedTyre);
            if (!isNewSelectionSameTypeAsOthers)
            {
                ClearIncompatibleTyreTypeSelections(tyrePlacementWithNewSelectedTyre);
            }
            UpdateResults();
        }

        private void ClearIncompatibleTyreFamilyOnSameAxle(TyrePlacementViewModel tyrePlacementWithNewSelectedTyre)
        {
            var tyreOnSameAxle = GetTyrePlacementOnSameAxleAs(tyrePlacementWithNewSelectedTyre);
            VoidPlacement(tyreOnSameAxle);
        }

        private void ClearIncompatibleTyreTypeSelections(TyrePlacementViewModel tyrePlacementWithNewSelectedTyre)
        {
            var placementsToClear = AllTyrePlacements.Where(p => p.SelectedTyre != null && p != tyrePlacementWithNewSelectedTyre);
            foreach (var placement in placementsToClear)
            {
                VoidPlacement(placement);
            }
        }

        private void VoidPlacement(TyrePlacementViewModel placement)
        {
            placement.SelectedTyre = null;
            placement.IsValid = false;
        }

        internal bool IsNewTyreOnSameAxleSameFamily(TyrePlacementViewModel tyrePlacementWithNewSelectedTyre)
        {
            var tyreOnSameAxle = GetTyrePlacementOnSameAxleAs(tyrePlacementWithNewSelectedTyre);
            return tyreOnSameAxle?.SelectedTyre == null || tyrePlacementWithNewSelectedTyre.SelectedTyre.IsSameFamilyAs(tyreOnSameAxle.SelectedTyre);
        }
        internal TyrePlacementViewModel GetTyrePlacementOnSameAxleAs(TyrePlacementViewModel tyrePlacementViewModel)
        {
            return AllTyrePlacements.FirstOrDefault(tyre => tyre != tyrePlacementViewModel && tyre.Axle == tyrePlacementViewModel.Axle);
        }

        internal bool IsNewTyreSameTypeAsOthers(TyrePlacementViewModel tyrePlacementWithNewSelectedTyre)
        {
            var placementsWithSelectedTyres = AllTyrePlacements.Where(t => t.SelectedTyre != null);
            return placementsWithSelectedTyres.All(t => t.SelectedTyre.Type == tyrePlacementWithNewSelectedTyre.SelectedTyre.Type);
        }

        /// <summary>
        /// Update the results for the tyre placements with a selected tyre as long as a track is selected.
        /// </summary>
        private void UpdateResults()
        {
            Task.Factory.StartNew(() => {
                if (SelectedTrack == null)
                {
                    // We need a track to work with
                    foreach (var tyrePlacement in AllTyrePlacements.Where(t => t.SelectedTyre != null))
                    {
                        tyrePlacement.IsValid = false;
                    }
                    return;
                }

                foreach (var tyrePlacement in AllTyrePlacements.Where(t => t.SelectedTyre != null))
                {
                    tyrePlacement.IsValid = true;
                    tyrePlacement.CalculateResults(SelectedTrack.Samples, SelectedTrackTemperature);
                }
            });
        }
      
        public void UpdateTemperature()
        {
            GetTemperatureForSelectedTrack();
        }
        private void GetTemperatureForSelectedTrack()
        {
            Debug.Assert(SelectedTrack != null);
            _weatherService.GetTemperatureForLocationAsync(SelectedTrack.Location, WeatherUtils_TemperatureUpdated);
        }

        private void WeatherUtils_TemperatureUpdated(object sender, EventArgs e)
        {
            if (e is TemperatureUpdatedEventArgs temperatureEventArgs)
            {
                SetNewTemperature(temperatureEventArgs.NewTemperature);
            }
        }

        private void SetNewTemperature(double newTemperature)
        {
            SelectedTrackTemperature = newTemperature;
        }
    }
}