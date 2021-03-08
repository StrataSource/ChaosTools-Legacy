using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ChaosInitiative.SDKLauncher.ViewModels;
using ChaosInitiative.SDKLauncher.Views;
using Steamworks;

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
            if (!(ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop))
                return;
            
            // Init steam stuff
            
            try
            {
                SteamClient.Init(440000); // TODO: Move this somewhere else
            }
            catch (Exception e)
            {
                if (!e.Message.Contains("Steam"))
                    throw;

                // TODO: This doesn't work well with i3wm
                desktop.MainWindow = new NotificationDialog("Steam error. Please check that steam is running.");
                Directory.CreateDirectory("logs");
                File.WriteAllText($"logs/steam_error_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.log", 
                                  e.Message );
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