using Xunit;
using VehicleManager.Core.Models;

namespace VehicleManager.Core.Tests
{
    public class VehicleTests
    {
        [Fact]
        public void Vehicle_ToString_ReturnsCorrectFormat()
        {
            // Arrange
            var vehicle = new Vehicle
            {
                Marke = "BMW",
                Modell = "X3",
                Baujahr = 2020
            };

            // Act
            var result = vehicle.ToString();

            // Assert
            Assert.Equal("BMW X3 (2020)", result);
        }

        [Theory]
        [InlineData(2020, 25000, 14762.25)] // 5 years old: 25000 * 0.9^5 = 14762.25
        [InlineData(2023, 30000, 24300)]    // 2 years old: 30000 * 0.9^2 = 24300
        [InlineData(2025, 20000, 20000)]    // Brand new: 20000 * 0.9^0 = 20000
        public void BerechneAktuellenWert_ReturnsCorrectDepreciatedValue(int baujahr, decimal kaufpreis, decimal expectedValue)
        {
            // Arrange
            var vehicle = new Vehicle
            {
                Baujahr = baujahr,
                Kaufpreis = kaufpreis
            };

            // Act
            var actualValue = vehicle.BerechneAktuellenWert();

            // Assert
            Assert.Equal(expectedValue, actualValue, 2); // 2 decimal places precision
        }

        [Fact]
        public void BerechneAktuellenWert_NeverGoesBelow10PercentOfPurchasePrice()
        {
            // Arrange - Very old car (50 years)
            var vehicle = new Vehicle
            {
                Baujahr = 1974,
                Kaufpreis = 10000
            };

            // Act
            var actualValue = vehicle.BerechneAktuellenWert();

            // Assert
            var minimumValue = vehicle.Kaufpreis * 0.1m; // 10% of purchase price
            Assert.True(actualValue >= minimumValue);
            Assert.Equal(1000, actualValue); // Should be exactly 10% = 1000
        }

        [Fact]
        public void Vehicle_Properties_CanBeSetAndRetrieved()
        {
            // Arrange & Act
            var vehicle = new Vehicle
            {
                Id = 1,
                Marke = "Mercedes-Benz",
                Modell = "A-Klasse",
                Baujahr = 2021,
                Kaufpreis = 32000.50m,
                Leistung = 163,
                Kilometerstand = 25000,
                Kraftstoff = "Benzin",
                Getriebe = "Automatik",
                Farbe = "Weiß",
                Kaufdatum = new DateTime(2021, 6, 15),
                Fahrzeugtyp = "PKW",
                Zusatzausstattung = "LED, Navigation"
            };

            // Assert
            Assert.Equal(1, vehicle.Id);
            Assert.Equal("Mercedes-Benz", vehicle.Marke);
            Assert.Equal("A-Klasse", vehicle.Modell);
            Assert.Equal(2021, vehicle.Baujahr);
            Assert.Equal(32000.50m, vehicle.Kaufpreis);
            Assert.Equal(163, vehicle.Leistung);
            Assert.Equal(25000, vehicle.Kilometerstand);
            Assert.Equal("Benzin", vehicle.Kraftstoff);
            Assert.Equal("Automatik", vehicle.Getriebe);
            Assert.Equal("Weiß", vehicle.Farbe);
            Assert.Equal(new DateTime(2021, 6, 15), vehicle.Kaufdatum);
            Assert.Equal("PKW", vehicle.Fahrzeugtyp);
            Assert.Equal("LED, Navigation", vehicle.Zusatzausstattung);
        }
    }
}