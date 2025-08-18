using System;

namespace VehicleManager.Core.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Marke { get; set; } = string.Empty;
        public string Modell { get; set; } = string.Empty;
        public int Baujahr { get; set; }
        public decimal Kaufpreis { get; set; }
        public int Leistung { get; set; } // PS
        public int Kilometerstand { get; set; }
        public string Kraftstoff { get; set; } = string.Empty;
        public string Getriebe { get; set; } = string.Empty;
        public string Farbe { get; set; } = string.Empty;
        public DateTime Kaufdatum { get; set; }
        public string Fahrzeugtyp { get; set; } = string.Empty; // PKW, LKW, Motorrad, etc.
        public string Zusatzausstattung { get; set; } = string.Empty;

        public decimal BerechneAktuellenWert()
        {
            int alter = DateTime.Now.Year - Baujahr;
            
            // Einfache Formel: 10% Wertverlust pro Jahr
            decimal wert = Kaufpreis;
            for (int i = 0; i < alter; i++)
            {
                wert = wert * 0.9m;
            }
            
            // Minimum 10% vom Kaufpreis
            if (wert < Kaufpreis * 0.1m)
                wert = Kaufpreis * 0.1m;
                
            return wert;
        }

        public override string ToString()
        {
            return $"{Marke} {Modell} ({Baujahr})";
        }
    }
}