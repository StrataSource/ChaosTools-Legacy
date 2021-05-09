using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ChaosInitiative.SDKLauncher.ViewModels;
using ChaosInitiative.SDKLauncher.Views;

namespace ChaosInitiative.SDKLauncher
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return;

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };

            base.OnFrameworkInitializationCompleted();
        }
    }
}