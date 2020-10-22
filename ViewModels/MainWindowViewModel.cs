using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using SDKLauncher.Models;
using SDKLauncher.Views;
using Steamworks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace SDKLauncher.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Profile> Profiles { get; set; }
        public Profile CurrentProfile { get; set; }

        public AppConfig Config { get; set; }
        
        public MainWindowViewModel()
        {
            SteamAPI.Init();

            try
            {
                Config = AppConfig.LoadConfig();
            } 
            catch (FileNotFoundException ex)
            {
                Config = AppConfig.CreateDefaultConfig();
                Config.Save();
            }

            Profiles = new ObservableCollection<Profile>(Config.Profiles);
            CurrentProfile = Profiles[Config.DefaultProfileIndex];
        }

        // ====================================

        public void OnClickHammer()
        {
            LaunchTool("hammer");
        }
        
        public void OnClickCreateMod()
        {
            CreateModWindow modOptions = new CreateModWindow();

            modOptions.ShowDialog(((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow);
        }

        public void OnClickModOptions()
        {
            ProfileConfigWindow modOptions = new ProfileConfigWindow();
            modOptions.DataContext = this;
            
            modOptions.ShowDialog( ((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow );
        }

        // ====================================

        private void LaunchTool(string executableName)
        {

            string extension;

            switch (System.Environment.OSVersion.Platform)
            {
                case System.PlatformID.Win32NT: 
                    extension = ".exe";
                break;
                default:
                    extension = "";
                break;
            }

            string binDir = CurrentProfile.Mod.Mount.GetBinDirectory();

            string executablePath = $"{binDir}/{executableName}{extension}";

            Process.Start(executablePath);

        }

    }
}
