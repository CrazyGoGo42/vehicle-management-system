using Xunit;
using VehicleManager.Core.Data;
using VehicleManager.Core.Models;
using VehicleManager.Core.ViewModels;
using System;

namespace VehicleManager.Core.Tests
{
    public class DatabaseIntegrationTests
    {
        [Fact]
        public void VehicleDatabase_CanConnect()
        {
            try
            {
                var database = new VehicleDatabase();
                var vehicles = database.GetAllVehicles();
                
                // Should be able to get vehicles without error
                Assert.NotNull(vehicles);
                Assert.True(vehicles.Count >= 0); // Allow empty database
            }
            catch (Exception ex)
            {
                // If this fails, it means database connection is not working
                Assert.True(false, $"Database connection failed: {ex.Message}");
            }
        }
        
        [Fact]
        public void MainViewModel_UsesOnlineDatabase()
        {
            try
            {
                var viewModel = new MainViewModel();
                
                // Load vehicles - should use online database since UseOfflineMode is false
                viewModel.LoadVehicles();
                
                // Should not throw exception and should have vehicles
                Assert.NotNull(viewModel.Vehicles);
                Assert.True(viewModel.StatusMessage.Contains("Connected") || viewModel.StatusMessage.Contains("Loaded"));
            }
            catch (Exception ex)
            {
                Assert.True(false, $"MainViewModel database integration failed: {ex.Message}");
            }
        }
        
        [Fact]
        public void VehicleDatabase_CanAddVehicle()
        {
            try
            {
                var database = new VehicleDatabase();
                var initialCount = database.GetAllVehicles().Count;
                
                var testVehicle = new Vehicle
                {
                    Marke = "IntegrationTest",
                    Modell = "TestModel",
                    Baujahr = 2024,
                    Kaufpreis = 25000,
                    Leistung = 150,
                    Kilometerstand = 0,
                    Kraftstoff = "Benzin",
                    Getriebe = "Automatik",
                    Farbe = "Blau",
                    Kaufdatum = DateTime.Now,
                    Fahrzeugtyp = "PKW",
                    Zusatzausstattung = "Integration Test Vehicle"
                };
                
                database.AddVehicle(testVehicle);
                
                var finalCount = database.GetAllVehicles().Count;
                Assert.True(finalCount > initialCount, "Vehicle should have been added to database");
            }
            catch (Exception ex)
            {
                Assert.True(false, $"Adding vehicle to database failed: {ex.Message}");
            }
        }
    }
}