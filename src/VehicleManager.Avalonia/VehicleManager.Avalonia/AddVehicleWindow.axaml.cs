using Avalonia.Controls;
using Avalonia.Interactivity;
using VehicleManager.Core.Models;
using System;

namespace VehicleManager.Avalonia
{
    public partial class AddVehicleWindow : Window
    {
        public AddVehicleWindow()
        {
            InitializeComponent();
            
            SaveButton.Click += SaveButton_Click;
            CancelButton.Click += CancelButton_Click;
            
            // Standardwerte setzen
            KraftstoffComboBox.SelectedIndex = 0;
            GetriebeComboBox.SelectedIndex = 0;
            FahrzeugtypComboBox.SelectedIndex = 0;
        }

        private void SaveButton_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                var vehicle = new Vehicle();
                
                vehicle.Marke = MarkeTextBox.Text ?? "";
                vehicle.Modell = ModellTextBox.Text ?? "";
                
                if (int.TryParse(BaujahrTextBox.Text, out int baujahr))
                    vehicle.Baujahr = baujahr;
                else
                    throw new Exception("Bitte geben Sie ein gültiges Baujahr ein");
                
                if (decimal.TryParse(KaufpreisTextBox.Text, out decimal kaufpreis))
                    vehicle.Kaufpreis = kaufpreis;
                else
                    throw new Exception("Bitte geben Sie einen gültigen Kaufpreis ein");
                
                if (int.TryParse(LeistungTextBox.Text, out int leistung))
                    vehicle.Leistung = leistung;
                else
                    throw new Exception("Bitte geben Sie eine gültige Leistung ein");
                
                if (int.TryParse(KilometerstandTextBox.Text, out int km))
                    vehicle.Kilometerstand = km;
                else
                    throw new Exception("Bitte geben Sie einen gültigen Kilometerstand ein");
                
                vehicle.Kraftstoff = ((ComboBoxItem)KraftstoffComboBox.SelectedItem!)?.Content?.ToString() ?? "";
                vehicle.Getriebe = ((ComboBoxItem)GetriebeComboBox.SelectedItem!)?.Content?.ToString() ?? "";
                vehicle.Farbe = FarbeTextBox.Text ?? "";
                vehicle.Fahrzeugtyp = ((ComboBoxItem)FahrzeugtypComboBox.SelectedItem!)?.Content?.ToString() ?? "";
                vehicle.Kaufdatum = DateTime.Now;
                vehicle.Zusatzausstattung = "";
                
                Close(vehicle);
            }
            catch (Exception ex)
            {
                // Einfache Fehleranzeige
                var errorWindow = new Window()
                {
                    Title = "Fehler",
                    Width = 300,
                    Height = 150,
                    Content = new StackPanel
                    {
                        Margin = new global::Avalonia.Thickness(20),
                        Children =
                        {
                            new TextBlock { Text = ex.Message },
                            new Button { Content = "OK", Margin = new global::Avalonia.Thickness(0, 10, 0, 0) }
                        }
                    }
                };
                
                errorWindow.ShowDialog(this);
            }
        }

        private void CancelButton_Click(object? sender, RoutedEventArgs e)
        {
            Close(null);
        }
    }
}