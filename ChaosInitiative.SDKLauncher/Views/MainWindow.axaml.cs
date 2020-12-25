using System.ComponentModel;
using Avalonia.Markup.Xaml;
using ChaosInitiative.SDKLauncher.ViewModels;

namespace ChaosInitiative.SDKLauncher.Views
{
    public class MainWindow : BaseWindow
    {

        private MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;
        
        protected override void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnClosing(object? sender, CancelEventArgs e) =>
            ViewModel.OnCloseWindow(sender, e);
        
    }
}
