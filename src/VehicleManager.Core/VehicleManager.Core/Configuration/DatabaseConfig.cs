using System.Text.Json;

namespace VehicleManager.Core.Configuration
{
    public class DatabaseConfig
    {
        public string Server { get; set; } = "localhost";
        public int Port { get; set; } = 3306;
        public string Database { get; set; } = "Autovermietung";
        public string Username { get; set; } = "root";
        public string Password { get; set; } = "";
        public bool UseOfflineMode { get; set; } = true;
        
        public string GetConnectionString()
        {
            return $"Server={Server};Port={Port};Database={Database};Uid={Username};Pwd={Password};";
        }
        
        public static DatabaseConfig Load()
        {
            try
            {
                var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                if (File.Exists(configPath))
                {
                    var json = File.ReadAllText(configPath);
                    var config = JsonSerializer.Deserialize<DatabaseConfig>(json);
                    return config ?? new DatabaseConfig();
                }
            }
            catch (Exception)
            {
                // Fall back to default config
            }
            
            return new DatabaseConfig();
        }
        
        public void Save()
        {
            try
            {
                var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                var json = JsonSerializer.Serialize(this, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                File.WriteAllText(configPath, json);
            }
            catch (Exception)
            {
                // Handle save error
            }
        }
    }
}