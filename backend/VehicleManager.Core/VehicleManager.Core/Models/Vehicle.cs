using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;

namespace VehicleManager.Core.Models
{
    // this is my main car class - holds all the data about each vehicle
    // also implements some interfaces so the UI updates automatically when values change
    public class Vehicle : INotifyPropertyChanged, IDisposable
    {
        // all my private fields to store the actual data
        private int _id;
        private string _marke = string.Empty;
        private string _modell = string.Empty;
        private int _baujahr;
        private decimal _kaufpreis;
        private decimal _aktuellerWert;
        private int _leistung;
        private int _kilometerstand;
        private string _kraftstoff = string.Empty;
        private string _getriebe = string.Empty;
        private string _farbe = string.Empty;
        private DateTime _kaufdatum;
        private string _fahrzeugtyp = string.Empty;
        private string _zusatzausstattung = string.Empty;
        
        // timer for automatic value depreciation - updates every 30 seconds
        private System.Timers.Timer? _depreciationTimer;

        // all the public properties - these are what the UI binds to
        public int Id 
        { 
            get => _id; 
            set 
            { 
                _id = value; 
                OnPropertyChanged(); 
            } 
        }
        
        public string Marke 
        { 
            get => _marke; 
            set 
            { 
                _marke = value ?? string.Empty; 
                OnPropertyChanged(); 
            } 
        }
        
        public string Modell 
        { 
            get => _modell; 
            set 
            { 
                _modell = value ?? string.Empty; 
                OnPropertyChanged(); 
            } 
        }
        
        public int Baujahr 
        { 
            get => _baujahr; 
            set 
            { 
                _baujahr = value; 
                OnPropertyChanged();
                // Wenn das Baujahr geändert wird, Wert neu berechnen
                BerechneUndAktualisiereWert();
            } 
        }
        
        public decimal Kaufpreis 
        { 
            get => _kaufpreis; 
            set 
            { 
                _kaufpreis = value; 
                OnPropertyChanged();
                // Wenn der Kaufpreis geändert wird, Wert neu berechnen
                BerechneUndAktualisiereWert();
            } 
        }
        
        /// <summary>
        /// Der aktuelle Wert des Fahrzeugs, der sich automatisch über die Zeit ändert
        /// </summary>
        public decimal AktuellerWert 
        { 
            get => _aktuellerWert; 
            private set 
            { 
                _aktuellerWert = value; 
                OnPropertyChanged(); 
            } 
        }
        
        public int Leistung 
        { 
            get => _leistung; 
            set 
            { 
                _leistung = value; 
                OnPropertyChanged(); 
            } 
        }
        
        public int Kilometerstand 
        { 
            get => _kilometerstand; 
            set 
            { 
                _kilometerstand = value; 
                OnPropertyChanged(); 
            } 
        }
        
        public string Kraftstoff 
        { 
            get => _kraftstoff; 
            set 
            { 
                _kraftstoff = value ?? string.Empty; 
                OnPropertyChanged(); 
            } 
        }
        
        public string Getriebe 
        { 
            get => _getriebe; 
            set 
            { 
                _getriebe = value ?? string.Empty; 
                OnPropertyChanged(); 
            } 
        }
        
        public string Farbe 
        { 
            get => _farbe; 
            set 
            { 
                _farbe = value ?? string.Empty; 
                OnPropertyChanged(); 
            } 
        }
        
        public DateTime Kaufdatum 
        { 
            get => _kaufdatum; 
            set 
            { 
                _kaufdatum = value; 
                OnPropertyChanged(); 
            } 
        }
        
        public string Fahrzeugtyp 
        { 
            get => _fahrzeugtyp; 
            set 
            { 
                _fahrzeugtyp = value ?? string.Empty; 
                OnPropertyChanged(); 
            } 
        }
        
        public string Zusatzausstattung 
        { 
            get => _zusatzausstattung; 
            set 
            { 
                _zusatzausstattung = value ?? string.Empty; 
                OnPropertyChanged(); 
            } 
        }

        // constructor - this runs when we create a new car object
        /// <summary>
        /// Konstruktor - wird aufgerufen wenn ein neues Vehicle erstellt wird
        /// Für Studenten: Hier initialisieren wir unsere Objekt-Instanz
        /// </summary>
        public Vehicle()
        {
            // Startwerte setzen
            Kaufdatum = DateTime.Now;
            
            // Timer für automatische Wertminderung starten
            StartDepreciationTimer();
        }

        // Methoden für Wertberechnung
        /// <summary>
        /// Berechnet den aktuellen Wert des Fahrzeugs basierend auf Alter und anderen Faktoren
        /// Für Studenten: Das ist eine Geschäftslogik-Methode
        /// </summary>
        /// <returns>Der berechnete aktuelle Wert</returns>
        public decimal BerechneAktuellenWert()
        {
            // Schritt 1: Alter des Fahrzeugs bestimmen
            int alterInJahren = DateTime.Now.Year - Baujahr;
            
            // Schritt 2: Basis-Wertverlust pro Jahr (10%)
            decimal wert = Kaufpreis;
            
            // Für Studenten: Eine for-Schleife um den Wertverlust Jahr für Jahr zu berechnen
            for (int jahr = 0; jahr < alterInJahren; jahr++)
            {
                wert = wert * 0.90m; // 10% Wertverlust pro Jahr
            }
            
            // Schritt 3: Zusätzlicher Wertverlust durch Kilometerstand
            if (Kilometerstand > 100000)
            {
                decimal kilometerverlust = (Kilometerstand - 100000) * 0.0001m;
                wert = wert * (1 - kilometerverlust);
            }
            
            // Schritt 4: Minimum-Wert festlegen (nie weniger als 5% des Kaufpreises)
            decimal minimumWert = Kaufpreis * 0.05m;
            if (wert < minimumWert)
            {
                wert = minimumWert;
            }
            
            return Math.Round(wert, 2); // Auf 2 Dezimalstellen runden
        }
        
        /// <summary>
        /// Berechnet den Wert neu und aktualisiert die AktuellerWert-Eigenschaft
        /// </summary>
        private void BerechneUndAktualisiereWert()
        {
            AktuellerWert = BerechneAktuellenWert();
        }

        // Timer für automatische Wertminderung
        /// <summary>
        /// Startet den Timer für die automatische Wertminderung
        /// Für Studenten: Das zeigt, wie man Timer in C# verwendet
        /// </summary>
        private void StartDepreciationTimer()
        {
            try
            {
                // Timer erstellen - alle 30 Sekunden (für Demo-Zwecke)
                _depreciationTimer = new System.Timers.Timer(30000); // 30000 Millisekunden = 30 Sekunden
                
                // Event-Handler für Timer-Ereignis
                _depreciationTimer.Elapsed += OnDepreciationTimerElapsed;
                
                // Timer aktivieren
                _depreciationTimer.AutoReset = true; // Timer automatisch zurücksetzen
                _depreciationTimer.Enabled = true;
                
                // Ersten Wert sofort berechnen
                BerechneUndAktualisiereWert();
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung - für Studenten wichtig!
                System.Diagnostics.Debug.WriteLine($"Fehler beim Starten des Depreciation-Timers: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Diese Methode wird alle 30 Sekunden aufgerufen
        /// Für Studenten: Event-Handler Pattern
        /// </summary>
        private void OnDepreciationTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            try
            {
                // Kilometerstand leicht erhöhen (Simulation)
                if (Kilometerstand < 500000) // Maximal 500.000 km
                {
                    _kilometerstand += new Random().Next(5, 25); // 5-25 km mehr
                    OnPropertyChanged(nameof(Kilometerstand));
                }
                
                // Wert neu berechnen
                BerechneUndAktualisiereWert();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler bei der automatischen Wertminderung: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Stoppt den Timer (wichtig für ordnungsgemäße Ressourcen-Freigabe)
        /// </summary>
        public void StopDepreciationTimer()
        {
            try
            {
                if (_depreciationTimer != null)
                {
                    _depreciationTimer.Stop();
                    _depreciationTimer.Dispose();
                    _depreciationTimer = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fehler beim Stoppen des Timers: {ex.Message}");
            }
        }

        // INotifyPropertyChanged Implementation
        // Für Studenten: Das ist ein wichtiges Interface für Data Binding in WPF/Avalonia
        public event PropertyChangedEventHandler? PropertyChanged;
        
        /// <summary>
        /// Benachrichtigt die UI über Eigenschaftsänderungen
        /// Für Studenten: Das CallerMemberName-Attribut automatisch den Namen der aufrufenden Property
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            // Null-Check für Thread-Sicherheit
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Override Methoden
        /// <summary>
        /// Überschreibt die ToString-Methode für bessere Lesbarkeit
        /// Für Studenten: Das ist ein wichtiges OOP-Konzept in C#
        /// </summary>
        public override string ToString()
        {
            return $"{Marke} {Modell} ({Baujahr}) - Aktueller Wert: {AktuellerWert:C}";
        }

        // IDisposable für ordnungsgemäße Ressourcen-Freigabe
        /// <summary>
        /// Implementiert IDisposable für ordnungsgemäße Ressourcen-Freigabe
        /// Für Studenten: Wichtiges Pattern für Speicher-Management
        /// </summary>
        public void Dispose()
        {
            StopDepreciationTimer();
        }
    }
}