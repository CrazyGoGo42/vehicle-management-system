# 🚗 Vehicle Manager - Cross-Platform Vehicle Management System

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Linux%20%7C%20macOS-lightgrey.svg)](https://github.com/avaloniaui/Avalonia)

A modern, cross-platform vehicle management application built with .NET 8, featuring both WPF and Avalonia UI implementations. Perfect for learning MVVM patterns, database integration, and cross-platform development.

## 🌟 Features

- **📱 Cross-Platform**: Runs on Windows, Linux, and macOS
- **🎨 Dual UI**: WPF implementation for Windows and Avalonia for cross-platform support
- **🗄️ Database Integration**: MySQL support with phpMyAdmin compatibility
- **💰 Value Calculation**: Automatic vehicle depreciation calculation
- **🔍 Search Functionality**: Find vehicles by brand, model, or other criteria
- **➕ CRUD Operations**: Add, view, edit, and delete vehicles
- **📊 MVVM Architecture**: Clean separation of concerns
- **🔧 Configurable**: Easy database configuration and offline mode

## 🏗️ Architecture

```
VehicleManager/
├── 📁 src/
│   ├── 🔧 VehicleManager.Core/          # Shared business logic and data access
│   ├── 🖥️ VehicleManager.WPF/           # WPF UI (Windows-specific)
│   └── 🌍 VehicleManager.Avalonia/      # Cross-platform UI
├── 📄 database_setup.sql               # Database schema and sample data
├── 📋 VehicleManager.sln               # Solution file
└── 📚 README.md                        # This file
```

## 🚀 Quick Start

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- MySQL Server (optional - includes offline mode)
- Git

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/vehicle-manager.git
   cd vehicle-manager
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Run the application**
   
   **Cross-Platform GUI (Recommended):**
   ```bash
   dotnet run --project src/VehicleManager.Avalonia/VehicleManager.Avalonia
   ```
   
   **Console Application:**
   ```bash
   dotnet run --project src/VehicleManager.WPF/VehicleManager.WPF
   ```

## 🗄️ Database Setup

### Option 1: MySQL Database

1. **Import database schema**
   - Open phpMyAdmin
   - Create database `Autovermietung` (or use existing)
   - Import `database_setup.sql`

2. **Configure connection string**
   
   Edit `src/VehicleManager.Core/Data/VehicleDatabase.cs`:
   ```csharp
   private string connectionString = "Server=localhost;Database=Autovermietung;Uid=root;Pwd=YOUR_PASSWORD;";
   ```

3. **Switch to MySQL mode**
   
   In `src/VehicleManager.Core/ViewModels/MainViewModel.cs`:
   ```csharp
   // Change from:
   database = new OfflineVehicleDatabase();
   // To:
   database = new VehicleDatabase();
   ```

### Option 2: Offline Mode (Default)

The application comes pre-configured with sample data and runs without a database connection. Perfect for testing and development!

## 🎯 Core Classes

### Vehicle Model
```csharp
public class Vehicle
{
    public int Id { get; set; }
    public string Marke { get; set; }        // Brand
    public string Modell { get; set; }       // Model
    public int Baujahr { get; set; }         // Year
    public decimal Kaufpreis { get; set; }   // Purchase Price
    public int Leistung { get; set; }        // Power (HP)
    public int Kilometerstand { get; set; }  // Mileage
    // ... and more properties
    
    public decimal BerechneAktuellenWert()   // Calculate current value
    {
        // 10% depreciation per year
        int age = DateTime.Now.Year - Baujahr;
        decimal value = Kaufpreis;
        
        for (int i = 0; i < age; i++)
        {
            value *= 0.9m;
        }
        
        return Math.Max(value, Kaufpreis * 0.1m); // Minimum 10% of purchase price
    }
}
```

## 🛠️ Technologies Used

- **Framework**: .NET 8
- **UI Frameworks**: 
  - Avalonia UI (Cross-platform)
  - WPF (Windows-specific)
- **Database**: MySQL with MySql.Data connector
- **Architecture**: MVVM (Model-View-ViewModel)
- **Language**: C# 12

## 📸 Screenshots

*Coming soon - Screenshots of the Avalonia and WPF interfaces*

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📋 TODO / Roadmap

- [ ] Add unit tests
- [ ] Implement PDF export functionality
- [ ] Add vehicle image support
- [ ] Implement data export (CSV, Excel)
- [ ] Add advanced search filters
- [ ] Localization support
- [ ] Dark/Light theme support

## 🐛 Troubleshooting

### Common Issues

**"Authentication failed" error:**
- Check MySQL credentials in connection string
- Ensure MySQL server is running
- Verify database exists
- Try switching to offline mode for testing

**"Cannot find database" error:**
- Import `database_setup.sql` to create tables
- Check database name in connection string

**UI not displaying:**
- Ensure .NET 8 SDK is installed
- Check console output for error messages
- Try rebuilding: `dotnet clean && dotnet build`

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Built for educational purposes
- Demonstrates MVVM pattern implementation
- Showcases cross-platform .NET development
- MySQL integration example

---

**Made with ❤️ for learning .NET development**