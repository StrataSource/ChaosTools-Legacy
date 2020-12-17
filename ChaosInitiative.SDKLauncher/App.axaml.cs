using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SDKLauncher.Steam;
using SDKLauncher.ViewModels;
using SDKLauncher.Views;
using Steamworks;

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
            
            // Init steam stuff
            
            try
            {
                SteamAPI.Init();
                if (!SteamAPI.IsSteamRunning())
                {
                    // TODO: This doesn't work well with i3wm
                    desktop.MainWindow = new SteamErrorDialog(SteamExceptionType.NotRunning);
                    return;
                }
            }
            catch (DllNotFoundException e)
            {
                if (!e.Message.Contains("steam"))
                    throw;

                // TODO: This doesn't work well with i3wm
                desktop.MainWindow = new SteamErrorDialog(SteamExceptionType.ApiNotFound);
                return;
            }

            // Steam works, launch application

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };

            base.OnFrameworkInitializationCompleted();
        }
    }
}