using System.Diagnostics;
using System.Windows.Controls;
using ViewModels;

namespace Views.Selections
{
    /// <summary>
    /// Interaction logic for SelectionsView.xaml
    /// </summary>
    public partial class SelectionsView : UserControl
    {
        public SelectionsView()
        {
            InitializeComponent();
        }

        private void TracksComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var mainViewModel = DataContext as MainViewModel;
            if (mainViewModel == null)
            {
                Debug.WriteLine("SelectionsView Error | TracksComboBox_OnSelectionChanged | Unexpected DataContext type. Expected type is MainViewModel.");
                return;
            }
            var tracksComboBox = sender as ComboBox;
            if (tracksComboBox == null)
            {
                Debug.WriteLine("SelectionsView Error | TracksComboBox_OnSelectionChanged | Unkown sender.");
                return;
            }
            var selectedTrack = tracksComboBox.SelectedValue as TrackDetailsViewModel;
            if (selectedTrack == null)
            {
                Debug.WriteLine("SelectionsView Error | TracksComboBox_OnSelectionChanged | SelectedValue should be of type TrackDetailsViewModel.");
                return;
            }
            mainViewModel.CurrentSelectionViewModel.SelectedTrack = selectedTrack;
            mainViewModel.CurrentSelectionViewModel.UpdateTemperature();
        }
     
        private void AllowedTyresComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tyresComboBox = e.OriginalSource as ComboBox;
            if (tyresComboBox == null)
            {
                Debug.WriteLine("SelectionsView Error | Unexpected event original source - expecting ComboBox.");
                return;
            }
            var selectedTyre = tyresComboBox.SelectedValue as TyreDetailsViewModel;
            if (selectedTyre == null)
            {
                // This is to reset the selection.
                tyresComboBox.SelectedIndex = -1;
            }
        }
    }
}