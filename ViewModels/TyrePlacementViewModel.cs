using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using ViewModelUtils.Enums;
using ViewModelUtils.Interfaces;

namespace ViewModels
{
    /// <summary>
    /// ViewModel that represents a tyre placement such as Front Left and Front Right.
    /// <para>This holds what tyres can be put on this placement, the current tyre on the</para>
    /// <para>placement and whether the current placement is valid or not.</para>
    /// <para>This is responsible for calculating the tyre degradation values for the selected</para>
    /// <para>for the selected tyre, and thereafter the <see cref="Average"/>, <see cref="Mode"/> and <see cref="Range"/>.</para>
    /// </summary>
    public class TyrePlacementViewModel : ViewModelBase, IIsOnAxle, IHasValidState
    {
        private TyrePlacement _tyrePlacementTitle;
        private bool _isValid;
        private TyreDetailsViewModel _selectedTyre;
        private ReadOnlyObservableCollection<TyreDetailsViewModel> _allowedTyres;
        private readonly ReaderWriterLockSlim _readerWriterLockSlim = new ReaderWriterLockSlim();

        public TyrePlacement TyrePlacementTitle
        {
            get
            {
                return _tyrePlacementTitle;
            }
            private set
            {
                _tyrePlacementTitle = value;
                OnPropertyChanged("TyrePlacement");
            }
        }
        public Axle Axle { get; }
        public ReadOnlyObservableCollection<TyreDetailsViewModel> AllowedTyres
        {
            get { return _allowedTyres; }
            private set
            {
                _allowedTyres = value; 
                OnPropertyChanged("AllowedTyres");
            }
        }
        public TyreDetailsViewModel SelectedTyre
        {
            get
            {
                _readerWriterLockSlim.EnterReadLock();
                var selectedTyre = _selectedTyre;
                _readerWriterLockSlim.ExitReadLock();
                return selectedTyre;
            }
            set
            {
                if (_selectedTyre == value)
                {
                    return;
                }
                _readerWriterLockSlim.EnterWriteLock();
                _selectedTyre = value; 
                _readerWriterLockSlim.ExitWriteLock();
                OnPropertyChanged("SelectedTyre");
                OnSelectedTyreChanged();
            }
        }
        public ValueStateViewModel Average { get; }
        public ValueStateViewModel Mode { get; }
        public ValueStateViewModel Range { get; }

        public bool IsValid
        {
            get
            {
                _readerWriterLockSlim.EnterReadLock();
                var isValid = _isValid;
                _readerWriterLockSlim.ExitReadLock();
                return isValid;
            }
            set
            {
                if (_isValid == value)
                {
                    return;
                }
                _readerWriterLockSlim.EnterWriteLock();
                _isValid = value;
                _readerWriterLockSlim.ExitWriteLock();
                OnPropertyChanged("IsValid");
                OnIsValidChanged();
            }
        }

        public TyrePlacementViewModel(TyrePlacement placement)
        {
            TyrePlacementTitle = placement;
            Average = new ValueStateViewModel("Average", this);
            Mode = new ValueStateViewModel("Mode", this);
            Range = new ValueStateViewModel("Range", this);
            Axle = placement.GetAxleByTyrePlacement();
        }

        internal void UpdateAllowedTyresCollection(List<TyreDetailsViewModel> availableTyres)
        {
            if (availableTyres == null)
            {
                throw new ArgumentNullException("TyrePlacementViewModel Error | availableTyres should not be null.");
            }
            var allowedTyres = new ObservableCollection<TyreDetailsViewModel>(availableTyres.FindAll(t => t.Placement == TyrePlacementTitle));
            CreateAllowedTyresReadonlyObservableCollection(allowedTyres);
        }
        private void CreateAllowedTyresReadonlyObservableCollection(ObservableCollection<TyreDetailsViewModel> allowedTyres)
        {
            AllowedTyres = new ReadOnlyObservableCollection<TyreDetailsViewModel>(allowedTyres);
        }

        internal void CalculateResults(ReadOnlyCollection<double> selectedTrackSamples, double selectedTrackTemperature)
        {
            double pointTyreDegradation;
            double sumOfAllPointDegradationValues = 0;
            double? biggestValue = null;
            double? smallestValue = null;
            var modeTally = new Dictionary<int, int>();
            foreach (var trackDegradationPoint in selectedTrackSamples)
            {
                pointTyreDegradation = CalculatePointTyreDegradation(trackDegradationPoint, SelectedTyre.TyreCoefficient, selectedTrackTemperature);

                sumOfAllPointDegradationValues += pointTyreDegradation;

                modeTally[(int) pointTyreDegradation] = modeTally.Keys.Contains((int) pointTyreDegradation)
                    ? modeTally[(int) pointTyreDegradation] + 1
                    : 1;

                biggestValue = biggestValue == null
                    ? pointTyreDegradation
                    : Math.Max((double)biggestValue, pointTyreDegradation);
                smallestValue = smallestValue == null
                    ? pointTyreDegradation
                    : Math.Min((double)smallestValue, pointTyreDegradation);

            }
            var average = sumOfAllPointDegradationValues / selectedTrackSamples.Count;
            Average.UpdateStateValue((int)average);
            Mode.UpdateStateValue(modeTally.First(m => m.Value == modeTally.Values.Max()).Key);
            if (biggestValue.HasValue)
            {
                Range.UpdateStateValue((int)(biggestValue - smallestValue));
            }
        }

        internal static double CalculatePointTyreDegradation(double trackDegradationPoint, double tyreCoefficient, double selectedTrackTemperature)
        {
            return (trackDegradationPoint + selectedTrackTemperature) / tyreCoefficient;
        }

        public event EventHandler IsValidChanged;
        protected virtual void OnIsValidChanged()
        {
            IsValidChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler SelectedTyreChanged;
        protected virtual void OnSelectedTyreChanged()
        {
            SelectedTyreChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}