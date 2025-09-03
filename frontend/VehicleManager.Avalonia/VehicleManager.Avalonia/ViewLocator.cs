using Avalonia.Controls;
using Avalonia.Controls.Templates;
using VehicleManager.Core.ViewModels;

namespace VehicleManager.Avalonia
{
    public class ViewLocator : IDataTemplate
    {
        public Control Build(object data)
        {
            return new TextBlock { Text = "Data template for: " + data?.GetType().Name };
        }

        public bool Match(object data)
        {
            return data is MainViewModel;
        }
    }
}