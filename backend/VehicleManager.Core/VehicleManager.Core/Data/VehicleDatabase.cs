using MySql.Data.MySqlClient;
using VehicleManager.Core.Models;

namespace VehicleManager.Core.Data
{
    public class VehicleDatabase
    {
        // Für Studenten: Connection String für MySQL-Datenbank
        // Beachte: Port 3307 weil der Standard-Port 3306 bereits belegt ist
        private string connectionString = "Server=localhost;Port=3307;Database=vehiclemanager;Uid=root;Pwd=1234;";

        public List<Vehicle> GetAllVehicles()
        {
            var vehicles = new List<Vehicle>();
            
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM vehicles";
                
                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var vehicle = new Vehicle();
                            vehicle.Id = reader.GetInt32("Id");
                            vehicle.Marke = reader.GetString("Marke");
                            vehicle.Modell = reader.GetString("Modell");
                            vehicle.Baujahr = reader.GetInt32("Baujahr");
                            vehicle.Kaufpreis = reader.GetDecimal("Kaufpreis");
                            vehicle.Leistung = reader.GetInt32("Leistung");
                            vehicle.Kilometerstand = reader.GetInt32("Kilometerstand");
                            vehicle.Kraftstoff = reader.GetString("Kraftstoff");
                            vehicle.Getriebe = reader.GetString("Getriebe");
                            vehicle.Farbe = reader.GetString("Farbe");
                            vehicle.Kaufdatum = reader.GetDateTime("Kaufdatum");
                            vehicle.Fahrzeugtyp = reader.GetString("Fahrzeugtyp");
                            vehicle.Zusatzausstattung = reader.GetString("Zusatzausstattung");
                            
                            vehicles.Add(vehicle);
                        }
                    }
                }
            }
            
            return vehicles;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = @"INSERT INTO vehicles (Marke, Modell, Baujahr, Kaufpreis, Leistung, 
                              Kilometerstand, Kraftstoff, Getriebe, Farbe, Kaufdatum, Fahrzeugtyp, Zusatzausstattung) 
                              VALUES (@marke, @modell, @baujahr, @kaufpreis, @leistung, @kilometerstand, 
                              @kraftstoff, @getriebe, @farbe, @kaufdatum, @fahrzeugtyp, @zusatzausstattung)";
                
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@marke", vehicle.Marke);
                    command.Parameters.AddWithValue("@modell", vehicle.Modell);
                    command.Parameters.AddWithValue("@baujahr", vehicle.Baujahr);
                    command.Parameters.AddWithValue("@kaufpreis", vehicle.Kaufpreis);
                    command.Parameters.AddWithValue("@leistung", vehicle.Leistung);
                    command.Parameters.AddWithValue("@kilometerstand", vehicle.Kilometerstand);
                    command.Parameters.AddWithValue("@kraftstoff", vehicle.Kraftstoff);
                    command.Parameters.AddWithValue("@getriebe", vehicle.Getriebe);
                    command.Parameters.AddWithValue("@farbe", vehicle.Farbe);
                    command.Parameters.AddWithValue("@kaufdatum", vehicle.Kaufdatum);
                    command.Parameters.AddWithValue("@fahrzeugtyp", vehicle.Fahrzeugtyp);
                    command.Parameters.AddWithValue("@zusatzausstattung", vehicle.Zusatzausstattung);
                    
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Vehicle> SearchVehicles(string searchText)
        {
            var vehicles = new List<Vehicle>();
            
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM vehicles WHERE Marke LIKE @search OR Modell LIKE @search";
                
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@search", "%" + searchText + "%");
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var vehicle = new Vehicle();
                            vehicle.Id = reader.GetInt32("Id");
                            vehicle.Marke = reader.GetString("Marke");
                            vehicle.Modell = reader.GetString("Modell");
                            vehicle.Baujahr = reader.GetInt32("Baujahr");
                            vehicle.Kaufpreis = reader.GetDecimal("Kaufpreis");
                            vehicle.Leistung = reader.GetInt32("Leistung");
                            vehicle.Kilometerstand = reader.GetInt32("Kilometerstand");
                            vehicle.Kraftstoff = reader.GetString("Kraftstoff");
                            vehicle.Getriebe = reader.GetString("Getriebe");
                            vehicle.Farbe = reader.GetString("Farbe");
                            vehicle.Kaufdatum = reader.GetDateTime("Kaufdatum");
                            vehicle.Fahrzeugtyp = reader.GetString("Fahrzeugtyp");
                            vehicle.Zusatzausstattung = reader.GetString("Zusatzausstattung");
                            
                            vehicles.Add(vehicle);
                        }
                    }
                }
            }
            
            return vehicles;
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = @"UPDATE vehicles SET 
                              Marke = @marke, Modell = @modell, Baujahr = @baujahr, 
                              Kaufpreis = @kaufpreis, Leistung = @leistung, Kilometerstand = @kilometerstand, 
                              Kraftstoff = @kraftstoff, Getriebe = @getriebe, Farbe = @farbe, 
                              Kaufdatum = @kaufdatum, Fahrzeugtyp = @fahrzeugtyp, Zusatzausstattung = @zusatzausstattung,
                              updated_at = CURRENT_TIMESTAMP
                              WHERE Id = @id";
                
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", vehicle.Id);
                    command.Parameters.AddWithValue("@marke", vehicle.Marke);
                    command.Parameters.AddWithValue("@modell", vehicle.Modell);
                    command.Parameters.AddWithValue("@baujahr", vehicle.Baujahr);
                    command.Parameters.AddWithValue("@kaufpreis", vehicle.Kaufpreis);
                    command.Parameters.AddWithValue("@leistung", vehicle.Leistung);
                    command.Parameters.AddWithValue("@kilometerstand", vehicle.Kilometerstand);
                    command.Parameters.AddWithValue("@kraftstoff", vehicle.Kraftstoff);
                    command.Parameters.AddWithValue("@getriebe", vehicle.Getriebe);
                    command.Parameters.AddWithValue("@farbe", vehicle.Farbe);
                    command.Parameters.AddWithValue("@kaufdatum", vehicle.Kaufdatum);
                    command.Parameters.AddWithValue("@fahrzeugtyp", vehicle.Fahrzeugtyp);
                    command.Parameters.AddWithValue("@zusatzausstattung", vehicle.Zusatzausstattung);
                    
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteVehicle(int vehicleId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = "DELETE FROM vehicles WHERE Id = @id";
                
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", vehicleId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}