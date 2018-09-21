using System;
using System.ComponentModel;
using System.Windows;

namespace ViewModels
{
    /// <summary>
    /// Base class for all ViewModels. Implements <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// This is for when we need to access the Dispatcher thread to update our ViewModelBase instances.
        /// </summary>
        /// <param name="action"></param>
        public void UpdateOnDispatcherThread(Action action)
        {
            // We need to access the  thread to update our Collection since the update instrument
           Application.Current.Dispatcher.Invoke(action);
        }
    }
}