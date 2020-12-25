using Avalonia.Markup.Xaml;
using ChaosInitiative.SDKLauncher.ViewModels;

namespace ChaosInitiative.SDKLauncher.Views
{
    public class MainWindow : BaseWindow
    {
        protected override void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
