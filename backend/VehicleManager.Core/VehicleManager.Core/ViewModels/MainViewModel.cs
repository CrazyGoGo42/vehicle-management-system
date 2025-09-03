using System.Collections.ObjectModel;
using System.ComponentModel;
using VehicleManager.Core.Data;
using VehicleManager.Core.Models;
using VehicleManager.Core.Configuration;
using VehicleManager.Core.Services;

namespace VehicleManager.Core.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private OfflineVehicleDatabase? offlineDatabase;
        private VehicleDatabase? onlineDatabase;
        private VehicleApiService? apiService;
        private DatabaseConfig config;
        private string _searchText = "";
        private Vehicle? _selectedVehicle;
        private string _statusMessage = "Ready";
        private bool _isLoading = false;

        public ObservableCollection<Vehicle> Vehicles { get; set; }
        
        public string SearchText 
        { 
            get => _searchText; 
            set 
            { 
                _searchText = value; 
                OnPropertyChanged(nameof(SearchText));
            } 
        }

        public Vehicle? SelectedVehicle 
        { 
            get => _selectedVehicle; 
            set 
            { 
                _selectedVehicle = value; 
                OnPropertyChanged(nameof(SelectedVehicle));
            } 
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public MainViewModel()
        {
            config = DatabaseConfig.Load();
            Vehicles = new ObservableCollection<Vehicle>();
            InitializeDatabase();
            LoadVehicles();
        }

        private void InitializeDatabase()
        {
            try
            {
                if (config.UseOfflineMode)
                {
                    offlineDatabase = new OfflineVehicleDatabase();
                    StatusMessage = "Using offline mode";
                }
                else
                {
                    // Use API service instead of direct database connection
                    apiService = new VehicleApiService();
                    StatusMessage = "Connected to API";
                }
            }
            catch (Exception ex)
            {
                // Fall back to offline mode
                offlineDatabase = new OfflineVehicleDatabase();
                StatusMessage = $"API connection failed - using offline mode: {ex.Message}";
            }
        }

        public async void LoadVehicles()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Loading vehicles...";

                List<Vehicle> vehicles;
                if (offlineDatabase != null)
                {
                    vehicles = offlineDatabase.GetAllVehicles();
                }
                else if (apiService != null)
                {
                    vehicles = await apiService.GetAllVehiclesAsync();
                }
                else
                {
                    throw new InvalidOperationException("No data source available");
                }

                Vehicles.Clear();
                foreach (var vehicle in vehicles)
                {
                    Vehicles.Add(vehicle);
                }

                StatusMessage = $"Loaded {vehicles.Count} vehicles";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading vehicles: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async void SearchVehicles()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Searching...";

                if (string.IsNullOrEmpty(SearchText))
                {
                    LoadVehicles();
                    return;
                }

                List<Vehicle> searchResults;
                if (offlineDatabase != null)
                {
                    searchResults = offlineDatabase.SearchVehicles(SearchText);
                }
                else if (apiService != null)
                {
                    searchResults = await apiService.SearchVehiclesAsync(SearchText);
                }
                else
                {
                    throw new InvalidOperationException("No data source available");
                }

                Vehicles.Clear();
                foreach (var vehicle in searchResults)
                {
                    Vehicles.Add(vehicle);
                }

                StatusMessage = $"Found {searchResults.Count} vehicles matching '{SearchText}'";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Search error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async void AddNewVehicle(Vehicle newVehicle)
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Adding vehicle...";

                if (offlineDatabase != null)
                {
                    offlineDatabase.AddVehicle(newVehicle);
                }
                else if (apiService != null)
                {
                    await apiService.CreateVehicleAsync(newVehicle);
                }
                else
                {
                    throw new InvalidOperationException("No data source available");
                }

                LoadVehicles();
                StatusMessage = "Vehicle added successfully";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error adding vehicle: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async void UpdateVehicle(Vehicle updatedVehicle)
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Updating vehicle...";

                if (offlineDatabase != null)
                {
                    offlineDatabase.UpdateVehicle(updatedVehicle);
                }
                else if (apiService != null)
                {
                    await apiService.UpdateVehicleAsync(updatedVehicle);
                }
                else
                {
                    throw new InvalidOperationException("No data source available");
                }

                LoadVehicles();
                StatusMessage = "Vehicle updated successfully";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error updating vehicle: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async void DeleteVehicle(Vehicle vehicleToDelete)
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Deleting vehicle...";

                if (offlineDatabase != null)
                {
                    offlineDatabase.DeleteVehicle(vehicleToDelete.Id);
                }
                else if (apiService != null)
                {
                    await apiService.DeleteVehicleAsync(vehicleToDelete.Id);
                }
                else
                {
                    throw new InvalidOperationException("No data source available");
                }

                LoadVehicles();
                StatusMessage = "Vehicle deleted successfully";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error deleting vehicle: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}