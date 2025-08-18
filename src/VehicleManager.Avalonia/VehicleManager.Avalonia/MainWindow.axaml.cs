using Avalonia.Controls;
using Avalonia.Interactivity;
using VehicleManager.Core.ViewModels;
using VehicleManager.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace VehicleManager.Avalonia;

public partial class MainWindow : Window
{
    private MainViewModel viewModel;
    
    public MainWindow()
    {
        InitializeComponent();
        viewModel = new MainViewModel();
        
        VehicleListBox.ItemsSource = viewModel.Vehicles;
        
        SearchButton.Click += SearchButton_Click;
        ShowAllButton.Click += ShowAllButton_Click;
        AddButton.Click += AddButton_Click;
        CalculateValueButton.Click += CalculateValueButton_Click;
        ExportPdfButton.Click += ExportPdfButton_Click;
    }

    private void SearchButton_Click(object? sender, RoutedEventArgs e)
    {
        viewModel.SearchText = SearchTextBox.Text ?? "";
        viewModel.SearchVehicles();
    }

    private void ShowAllButton_Click(object? sender, RoutedEventArgs e)
    {
        SearchTextBox.Text = "";
        viewModel.LoadVehicles();
    }

    private async void AddButton_Click(object? sender, RoutedEventArgs e)
    {
        var dialog = new AddVehicleWindow();
        var result = await dialog.ShowDialog<Vehicle?>(this);
        
        if (result != null)
        {
            viewModel.AddNewVehicle(result);
        }
    }

    private async void CalculateValueButton_Click(object? sender, RoutedEventArgs e)
    {
        var selectedVehicle = VehicleListBox.SelectedItem as Vehicle;
        if (selectedVehicle != null)
        {
            var currentValue = selectedVehicle.BerechneAktuellenWert();
            var message = $"Aktueller Wert: {currentValue:C}\n" +
                         $"Kaufpreis: {selectedVehicle.Kaufpreis:C}\n" +
                         $"Wertverlust: {selectedVehicle.Kaufpreis - currentValue:C}";
            
            var msgBox = new Window()
            {
                Title = "Fahrzeugwert",
                Width = 300,
                Height = 150,
                Content = new StackPanel
                {
                    Margin = new global::Avalonia.Thickness(20),
                    Children =
                    {
                        new TextBlock { Text = message },
                        new Button { Content = "OK", Margin = new global::Avalonia.Thickness(0, 10, 0, 0) }
                    }
                }
            };
            
            await msgBox.ShowDialog(this);
        }
    }

    private async void ExportPdfButton_Click(object? sender, RoutedEventArgs e)
    {
        var selectedVehicle = VehicleListBox.SelectedItem as Vehicle;
        if (selectedVehicle != null)
        {
            // Einfache PDF-Export Simulation
            var message = "PDF würde erstellt werden für:\n" +
                         $"{selectedVehicle.Marke} {selectedVehicle.Modell}\n" +
                         "Feature noch nicht implementiert";
            
            var msgBox = new Window()
            {
                Title = "PDF Export",
                Width = 300,
                Height = 150,
                Content = new TextBlock { Text = message, Margin = new global::Avalonia.Thickness(20) }
            };
            
            await msgBox.ShowDialog(this);
        }
    }
}