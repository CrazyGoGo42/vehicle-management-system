using System.Windows;
using System.Windows.Controls;
using VehicleManager.Core.Models;

namespace VehicleManager.WPF
{
    public partial class EditVehicleWindow : Window
    {
        private Vehicle originalVehicle;
        public Vehicle? Vehicle { get; private set; }

        public EditVehicleWindow(Vehicle vehicle)
        {
            InitializeComponent();
            originalVehicle = vehicle;
            LoadVehicleData();
        }

        private void LoadVehicleData()
        {
            if (originalVehicle == null) return;

            MarkeTextBox.Text = originalVehicle.Marke;
            ModellTextBox.Text = originalVehicle.Modell;
            BaujahrTextBox.Text = originalVehicle.Baujahr.ToString();
            KaufpreisTextBox.Text = originalVehicle.Kaufpreis.ToString();
            LeistungTextBox.Text = originalVehicle.Leistung.ToString();
            KilometerstandTextBox.Text = originalVehicle.Kilometerstand.ToString();
            FarbeTextBox.Text = originalVehicle.Farbe;
            ZusatzausstattungTextBox.Text = originalVehicle.Zusatzausstattung;

            // Set ComboBox selections
            SetComboBoxSelection(KraftstoffComboBox, originalVehicle.Kraftstoff);
            SetComboBoxSelection(GetriebeComboBox, originalVehicle.Getriebe);
            SetComboBoxSelection(FahrzeugtypComboBox, originalVehicle.Fahrzeugtyp);
        }

        private void SetComboBoxSelection(ComboBox comboBox, string value)
        {
            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Content?.ToString() == value)
                {
                    comboBox.SelectedItem = item;
                    return;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var updatedVehicle = new Vehicle();
                
                // Keep the original ID and dates
                updatedVehicle.Id = originalVehicle.Id;
                updatedVehicle.Kaufdatum = originalVehicle.Kaufdatum;
                
                updatedVehicle.Marke = MarkeTextBox.Text ?? "";
                updatedVehicle.Modell = ModellTextBox.Text ?? "";
                
                if (int.TryParse(BaujahrTextBox.Text, out int baujahr))
                    updatedVehicle.Baujahr = baujahr;
                else
                    throw new Exception("Bitte geben Sie ein gültiges Baujahr ein");
                
                if (decimal.TryParse(KaufpreisTextBox.Text, out decimal kaufpreis))
                    updatedVehicle.Kaufpreis = kaufpreis;
                else
                    throw new Exception("Bitte geben Sie einen gültigen Kaufpreis ein");
                
                if (int.TryParse(LeistungTextBox.Text, out int leistung))
                    updatedVehicle.Leistung = leistung;
                else
                    throw new Exception("Bitte geben Sie eine gültige Leistung ein");
                
                if (int.TryParse(KilometerstandTextBox.Text, out int km))
                    updatedVehicle.Kilometerstand = km;
                else
                    throw new Exception("Bitte geben Sie einen gültigen Kilometerstand ein");
                
                updatedVehicle.Kraftstoff = ((ComboBoxItem)KraftstoffComboBox.SelectedItem!)?.Content?.ToString() ?? "";
                updatedVehicle.Getriebe = ((ComboBoxItem)GetriebeComboBox.SelectedItem!)?.Content?.ToString() ?? "";
                updatedVehicle.Farbe = FarbeTextBox.Text ?? "";
                updatedVehicle.Fahrzeugtyp = ((ComboBoxItem)FahrzeugtypComboBox.SelectedItem!)?.Content?.ToString() ?? "";
                updatedVehicle.Zusatzausstattung = ZusatzausstattungTextBox.Text ?? "";
                
                Vehicle = updatedVehicle;
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