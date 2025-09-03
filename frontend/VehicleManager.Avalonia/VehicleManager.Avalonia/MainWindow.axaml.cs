using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;
using VehicleManager.Core.ViewModels;
using VehicleManager.Core.Models;

namespace VehicleManager.Avalonia
{
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;
            
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object? sender, RoutedEventArgs e)
        {
            await Task.Run(() => viewModel.LoadVehicles());
        }

        private void SearchButton_Click(object? sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void SearchTextBox_KeyUp(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformSearch();
            }
        }

        private void PerformSearch()
        {
            viewModel.SearchText = SearchTextBox.Text ?? "";
            if (string.IsNullOrEmpty(SearchTextBox.Text))
            {
                viewModel.LoadVehicles();
            }
            else
            {
                viewModel.SearchVehicles();
            }
        }

        private async void AddButton_Click(object? sender, RoutedEventArgs e)
        {
            var addWindow = new AddVehicleWindow();
            var result = await addWindow.ShowDialog<Vehicle?>(this);
            if (result != null)
            {
                viewModel.AddNewVehicle(result);
            }
        }

        private async void EditButton_Click(object? sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedVehicle != null)
            {
                var editWindow = new EditVehicleWindow(viewModel.SelectedVehicle);
                var result = await editWindow.ShowDialog<Vehicle?>(this);
                if (result != null)
                {
                    viewModel.UpdateVehicle(result);
                }
            }
        }

        private void DeleteButton_Click(object? sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedVehicle != null)
            {
                // Simple confirmation for now
                viewModel.DeleteVehicle(viewModel.SelectedVehicle);
            }
        }

        private void VehiclesDataGrid_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                var selectedVehicle = dataGrid.SelectedItem as Vehicle;
                viewModel.SelectedVehicle = selectedVehicle;
                
                EditButton.IsEnabled = selectedVehicle != null;
                DeleteButton.IsEnabled = selectedVehicle != null;
            }
        }
    }
}