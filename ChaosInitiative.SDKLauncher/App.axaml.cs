using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SDKLauncher.ViewModels;
using SDKLauncher.Views;

namespace SDKLauncher
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (!(ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop))
                return;
            
            try
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }
            catch (DllNotFoundException e)
            {
                if (!e.Message.Contains("steam"))
                    throw;

                desktop.MainWindow = new SteamErrorDialog(); // TODO: This doesn't work well with i3wm

            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}