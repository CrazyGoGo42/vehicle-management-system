using VehicleManager.Core.ViewModels;
using VehicleManager.Core.Models;

namespace VehicleManager.WPF
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Fahrzeugverwaltung WPF Version ===");
            Console.WriteLine("(Simuliert da WPF unter Linux nicht läuft)");
            Console.WriteLine();

            var viewModel = new MainViewModel();
            
            while (true)
            {
                Console.WriteLine("1. Alle Fahrzeuge anzeigen");
                Console.WriteLine("2. Fahrzeug suchen");
                Console.WriteLine("3. Neues Fahrzeug hinzufügen");
                Console.WriteLine("4. Fahrzeugwert berechnen");
                Console.WriteLine("5. Beenden");
                Console.Write("Wählen Sie eine Option: ");
                
                var input = Console.ReadLine();
                
                switch (input)
                {
                    case "1":
                        ShowAllVehicles(viewModel);
                        break;
                    case "2":
                        SearchVehicles(viewModel);
                        break;
                    case "3":
                        AddNewVehicle(viewModel);
                        break;
                    case "4":
                        CalculateValue(viewModel);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Ungültige Eingabe!");
                        break;
                }
                
                Console.WriteLine("\nDrücken Sie eine Taste...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void ShowAllVehicles(MainViewModel viewModel)
        {
            Console.WriteLine("\n=== Alle Fahrzeuge ===");
            foreach (var vehicle in viewModel.Vehicles)
            {
                Console.WriteLine($"{vehicle.Id}: {vehicle.Marke} {vehicle.Modell} ({vehicle.Baujahr}) - {vehicle.Kaufpreis:C}");
            }
        }

        static void SearchVehicles(MainViewModel viewModel)
        {
            Console.Write("Suchbegriff eingeben: ");
            var searchTerm = Console.ReadLine() ?? "";
            viewModel.SearchText = searchTerm;
            viewModel.SearchVehicles();
            
            Console.WriteLine($"\n=== Suchergebnisse für '{searchTerm}' ===");
            foreach (var vehicle in viewModel.Vehicles)
            {
                Console.WriteLine($"{vehicle.Marke} {vehicle.Modell} ({vehicle.Baujahr})");
            }
        }

        static void AddNewVehicle(MainViewModel viewModel)
        {
            Console.WriteLine("\n=== Neues Fahrzeug hinzufügen ===");
            
            var vehicle = new Vehicle();
            
            Console.Write("Marke: ");
            vehicle.Marke = Console.ReadLine() ?? "";
            
            Console.Write("Modell: ");
            vehicle.Modell = Console.ReadLine() ?? "";
            
            Console.Write("Baujahr: ");
            if (int.TryParse(Console.ReadLine(), out int baujahr))
                vehicle.Baujahr = baujahr;
            
            Console.Write("Kaufpreis: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal preis))
                vehicle.Kaufpreis = preis;
            
            Console.Write("Leistung (PS): ");
            if (int.TryParse(Console.ReadLine(), out int leistung))
                vehicle.Leistung = leistung;
            
            Console.Write("Kilometerstand: ");
            if (int.TryParse(Console.ReadLine(), out int km))
                vehicle.Kilometerstand = km;
            
            Console.Write("Kraftstoff: ");
            vehicle.Kraftstoff = Console.ReadLine() ?? "";
            
            Console.Write("Getriebe: ");
            vehicle.Getriebe = Console.ReadLine() ?? "";
            
            Console.Write("Farbe: ");
            vehicle.Farbe = Console.ReadLine() ?? "";
            
            vehicle.Kaufdatum = DateTime.Now;
            vehicle.Fahrzeugtyp = "PKW";
            vehicle.Zusatzausstattung = "";
            
            viewModel.AddNewVehicle(vehicle);
            Console.WriteLine("Fahrzeug wurde hinzugefügt!");
        }

        static void CalculateValue(MainViewModel viewModel)
        {
            ShowAllVehicles(viewModel);
            Console.Write("\nFahrzeug-ID für Wertberechnung: ");
            
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var vehicle = viewModel.Vehicles.FirstOrDefault(v => v.Id == id);
                if (vehicle != null)
                {
                    var currentValue = vehicle.BerechneAktuellenWert();
                    Console.WriteLine($"\nAktueller Wert von {vehicle.Marke} {vehicle.Modell}: {currentValue:C}");
                    Console.WriteLine($"Kaufpreis war: {vehicle.Kaufpreis:C}");
                    Console.WriteLine($"Wertverlust: {vehicle.Kaufpreis - currentValue:C}");
                }
                else
                {
                    Console.WriteLine("Fahrzeug nicht gefunden!");
                }
            }
        }
    }
}
