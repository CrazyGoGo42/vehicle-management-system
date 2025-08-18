using VehicleManager.Core.Models;

namespace VehicleManager.Core.Data
{
    public class OfflineVehicleDatabase
    {
        private List<Vehicle> vehicles;

        public OfflineVehicleDatabase()
        {
            // Dummy-Daten für Demo
            vehicles = new List<Vehicle>
            {
                new Vehicle
                {
                    Id = 1,
                    Marke = "Volkswagen",
                    Modell = "Golf",
                    Baujahr = 2020,
                    Kaufpreis = 25000.00m,
                    Leistung = 150,
                    Kilometerstand = 45000,
                    Kraftstoff = "Benzin",
                    Getriebe = "Manuell",
                    Farbe = "Blau",
                    Kaufdatum = new DateTime(2020, 3, 15),
                    Fahrzeugtyp = "PKW",
                    Zusatzausstattung = "Klimaanlage, Navigationssystem"
                },
                new Vehicle
                {
                    Id = 2,
                    Marke = "BMW",
                    Modell = "X3",
                    Baujahr = 2019,
                    Kaufpreis = 45000.00m,
                    Leistung = 190,
                    Kilometerstand = 67000,
                    Kraftstoff = "Diesel",
                    Getriebe = "Automatik",
                    Farbe = "Schwarz",
                    Kaufdatum = new DateTime(2019, 7, 22),
                    Fahrzeugtyp = "SUV",
                    Zusatzausstattung = "Ledersitze, Panoramadach"
                },
                new Vehicle
                {
                    Id = 3,
                    Marke = "Mercedes-Benz",
                    Modell = "A-Klasse",
                    Baujahr = 2021,
                    Kaufpreis = 32000.00m,
                    Leistung = 163,
                    Kilometerstand = 23000,
                    Kraftstoff = "Benzin",
                    Getriebe = "Automatik",
                    Farbe = "Weiß",
                    Kaufdatum = new DateTime(2021, 1, 10),
                    Fahrzeugtyp = "PKW",
                    Zusatzausstattung = "MBUX, LED-Scheinwerfer"
                }
            };
        }

        public List<Vehicle> GetAllVehicles()
        {
            return vehicles.ToList();
        }

        public void AddVehicle(Vehicle vehicle)
        {
            vehicle.Id = vehicles.Max(v => v.Id) + 1;
            vehicles.Add(vehicle);
        }

        public List<Vehicle> SearchVehicles(string searchText)
        {
            return vehicles.Where(v => 
                v.Marke.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                v.Modell.Contains(searchText, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }
    }
}