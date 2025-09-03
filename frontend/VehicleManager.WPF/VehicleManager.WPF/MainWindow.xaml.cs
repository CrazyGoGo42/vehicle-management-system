using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VehicleManager.Core.ViewModels;
using VehicleManager.Core.Models;

namespace VehicleManager.WPF
{
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;
            
            // Load vehicles on startup
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => viewModel.LoadVehicles());
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformSearch();
            }
        }

        private void PerformSearch()
        {
            viewModel.SearchText = SearchTextBox.Text;
            if (string.IsNullOrEmpty(SearchTextBox.Text))
            {
                viewModel.LoadVehicles();
            }
            else
            {
                viewModel.SearchVehicles();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddVehicleWindow();
            if (addWindow.ShowDialog() == true)
            {
                var newVehicle = addWindow.Vehicle;
                if (newVehicle != null)
                {
                    viewModel.AddNewVehicle(newVehicle);
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedVehicle != null)
            {
                var editWindow = new EditVehicleWindow(viewModel.SelectedVehicle);
                if (editWindow.ShowDialog() == true)
                {
                    var updatedVehicle = editWindow.Vehicle;
                    if (updatedVehicle != null)
                    {
                        viewModel.UpdateVehicle(updatedVehicle);
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedVehicle != null)
            {
                var result = MessageBox.Show(
                    $"Möchten Sie das Fahrzeug '{viewModel.SelectedVehicle.Marke} {viewModel.SelectedVehicle.Modell}' wirklich löschen?",
                    "Fahrzeug löschen",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    viewModel.DeleteVehicle(viewModel.SelectedVehicle);
                }
            }
        }

        private void VehiclesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedVehicle = VehiclesDataGrid.SelectedItem as Vehicle;
            viewModel.SelectedVehicle = selectedVehicle;
            
            EditButton.IsEnabled = selectedVehicle != null;
            DeleteButton.IsEnabled = selectedVehicle != null;
        }
    }
}