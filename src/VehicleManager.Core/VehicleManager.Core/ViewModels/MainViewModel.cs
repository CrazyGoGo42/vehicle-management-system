using System.Collections.ObjectModel;
using System.ComponentModel;
using VehicleManager.Core.Data;
using VehicleManager.Core.Models;
using VehicleManager.Core.Configuration;

namespace VehicleManager.Core.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private OfflineVehicleDatabase? offlineDatabase;
        private VehicleDatabase? onlineDatabase;
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
                    onlineDatabase = new VehicleDatabase();
                    StatusMessage = "Connected to database";
                }
            }
            catch (Exception ex)
            {
                // Fall back to offline mode
                offlineDatabase = new OfflineVehicleDatabase();
                StatusMessage = $"Database connection failed - using offline mode: {ex.Message}";
            }
        }

        public void LoadVehicles()
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
                else if (onlineDatabase != null)
                {
                    vehicles = onlineDatabase.GetAllVehicles();
                }
                else
                {
                    throw new InvalidOperationException("No database available");
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

        public void SearchVehicles()
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
                else if (onlineDatabase != null)
                {
                    searchResults = onlineDatabase.SearchVehicles(SearchText);
                }
                else
                {
                    throw new InvalidOperationException("No database available");
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

        public void AddNewVehicle(Vehicle newVehicle)
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Adding vehicle...";

                if (offlineDatabase != null)
                {
                    offlineDatabase.AddVehicle(newVehicle);
                }
                else if (onlineDatabase != null)
                {
                    onlineDatabase.AddVehicle(newVehicle);
                }
                else
                {
                    throw new InvalidOperationException("No database available");
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}