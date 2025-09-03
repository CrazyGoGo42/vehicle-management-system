using System.Windows;
using System.Windows.Controls;
using VehicleManager.Core.Models;

namespace VehicleManager.WPF
{
    public partial class AddVehicleWindow : Window
    {
        public Vehicle? Vehicle { get; private set; }

        public AddVehicleWindow()
        {
            InitializeComponent();
            
            // Set default selections
            KraftstoffComboBox.SelectedIndex = 0;
            GetriebeComboBox.SelectedIndex = 0;
            FahrzeugtypComboBox.SelectedIndex = 0;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
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
                
                vehicle.Kraftstoff = ((ComboBoxItem)KraftstoffComboBox.SelectedItem)?.Content?.ToString() ?? "";
                vehicle.Getriebe = ((ComboBoxItem)GetriebeComboBox.SelectedItem)?.Content?.ToString() ?? "";
                vehicle.Farbe = FarbeTextBox.Text ?? "";
                vehicle.Fahrzeugtyp = ((ComboBoxItem)FahrzeugtypComboBox.SelectedItem)?.Content?.ToString() ?? "";
                vehicle.Zusatzausstattung = ZusatzausstattungTextBox.Text ?? "";
                vehicle.Kaufdatum = DateTime.Now;
                
                Vehicle = vehicle;
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}