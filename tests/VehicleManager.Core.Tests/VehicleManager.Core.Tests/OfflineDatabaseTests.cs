using Xunit;
using VehicleManager.Core.Data;
using VehicleManager.Core.Models;

namespace VehicleManager.Core.Tests
{
    public class OfflineDatabaseTests
    {
        [Fact]
        public void GetAllVehicles_ReturnsInitialData()
        {
            // Arrange
            var database = new OfflineVehicleDatabase();

            // Act
            var vehicles = database.GetAllVehicles();

            // Assert
            Assert.NotEmpty(vehicles);
            Assert.Equal(3, vehicles.Count); // We expect 3 initial vehicles
            Assert.Contains(vehicles, v => v.Marke == "Volkswagen" && v.Modell == "Golf");
            Assert.Contains(vehicles, v => v.Marke == "BMW" && v.Modell == "X3");
            Assert.Contains(vehicles, v => v.Marke == "Mercedes-Benz" && v.Modell == "A-Klasse");
        }

        [Fact]
        public void AddVehicle_IncreasesVehicleCount()
        {
            // Arrange
            var database = new OfflineVehicleDatabase();
            var initialCount = database.GetAllVehicles().Count;
            
            var newVehicle = new Vehicle
            {
                Marke = "Audi",
                Modell = "A4",
                Baujahr = 2022,
                Kaufpreis = 35000,
                Leistung = 150,
                Kilometerstand = 15000,
                Kraftstoff = "Benzin",
                Getriebe = "Automatik",
                Farbe = "Schwarz",
                Kaufdatum = DateTime.Now,
                Fahrzeugtyp = "PKW",
                Zusatzausstattung = "Premium Package"
            };

            // Act
            database.AddVehicle(newVehicle);
            var vehiclesAfterAdd = database.GetAllVehicles();

            // Assert
            Assert.Equal(initialCount + 1, vehiclesAfterAdd.Count);
            Assert.Contains(vehiclesAfterAdd, v => v.Marke == "Audi" && v.Modell == "A4");
        }

        [Fact]
        public void AddVehicle_AssignsUniqueId()
        {
            // Arrange
            var database = new OfflineVehicleDatabase();
            var existingVehicles = database.GetAllVehicles();
            var maxExistingId = existingVehicles.Max(v => v.Id);
            
            var newVehicle = new Vehicle
            {
                Marke = "Toyota",
                Modell = "Camry",
                Baujahr = 2023
            };

            // Act
            database.AddVehicle(newVehicle);

            // Assert
            Assert.Equal(maxExistingId + 1, newVehicle.Id);
        }

        [Theory]
        [InlineData("BMW", 1)] // Should find BMW X3
        [InlineData("Golf", 1)] // Should find Volkswagen Golf
        [InlineData("Mercedes", 1)] // Should find Mercedes-Benz A-Klasse
        [InlineData("NonExistent", 0)] // Should find nothing
        [InlineData("", 3)] // Empty search should return all
        public void SearchVehicles_ReturnsCorrectResults(string searchTerm, int expectedCount)
        {
            // Arrange
            var database = new OfflineVehicleDatabase();

            // Act
            var results = database.SearchVehicles(searchTerm);

            // Assert
            Assert.Equal(expectedCount, results.Count);
        }

        [Fact]
        public void SearchVehicles_IsCaseInsensitive()
        {
            // Arrange
            var database = new OfflineVehicleDatabase();

            // Act
            var lowerCaseResults = database.SearchVehicles("bmw");
            var upperCaseResults = database.SearchVehicles("BMW");
            var mixedCaseResults = database.SearchVehicles("Bmw");

            // Assert
            Assert.Equal(lowerCaseResults.Count, upperCaseResults.Count);
            Assert.Equal(upperCaseResults.Count, mixedCaseResults.Count);
            Assert.True(lowerCaseResults.Count > 0); // Should find BMW X3
        }
    }
}