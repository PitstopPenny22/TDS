using ViewModelUtils.Interfaces;

namespace ViewModels
{
    /// <summary>
    /// ViewModel that represents an entity with a value that corresponds to a state.
    /// </summary>
    public class ValueStateViewModel : ViewModelBase
    {
        public enum DegradationState
        {
            NotValid, // No valid selections
            Green,    // na - 999
            Yellow,   // 1000 - 2999
            Red       // 3000 - na
        }

        private readonly object _lockObject = new object();
        private int _value;
        private string _valueName;
        private DegradationState _state;
        internal const int GreenThreshold = 999;
        internal const int RedThreshold = 3000;
       
        public int Value
        {
            get { return _value; }
            private set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        /// <summary>
        /// The correspondin <see cref="DegradationState"/> for the current <see cref="Value"/>./>
        /// </summary>
        public DegradationState State
        {
            get
            {
                return _state;
            }
            private set
            {
                _state = value;
                OnPropertyChanged("State");
            }
        }

        /// <summary>
        /// What this ViewModel is representing.
        /// </summary>
        public string ValueName
        {
            get { return _valueName; }
            private set
            {
                _valueName = value;
                OnPropertyChanged("ValueName");
            }
        }

        public ValueStateViewModel(string valueName, IHasValidState parentValidState)
        {
            ValueName = valueName;
            parentValidState.IsValidChanged += ParentValidState_IsValidChanged;
        }

        private void ParentValidState_IsValidChanged(object sender, System.EventArgs e)
        {
            var parentState = sender as IHasValidState;
            if (parentState != null && !parentState.IsValid)
            {
               Invalidate();
            }
        }

        /// <summary>
        /// Set <see cref="State"/> to <see cref="DegradationState.NotValid"/> and <see cref="Value"/> to 0.
        /// </summary>
        internal void Invalidate()
        {
            State = DegradationState.NotValid;
            VoidValue();
        }

        private void VoidValue()
        {
            _value = 0;
            OnPropertyChanged("Value");
        }

        public void UpdateStateValue(int value)
        {
            lock (_lockObject)
            {
                Value = value;
                UpdateState();
            }
        }

        private void UpdateState()
        {
            if (Value <= GreenThreshold)
            {
                State = DegradationState.Green;
            }
            else if (Value < RedThreshold)
            {
                State = DegradationState.Yellow;
            }
            else
            {
                State = DegradationState.Red;
            }
        }
    }
}