using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using SDKLauncher.Models;
using SDKLauncher.Views;
using Steamworks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SDKLauncher.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Profile> Profiles { get; set; }
        public Profile CurrentProfile { get; set; }
        public Mount CurrentMount { get; set; }
        public AppConfig Config { get; set; }

        private Window MainWindow => ((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow;


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

            modOptions.ShowDialog(MainWindow);
        }

        public async Task OnClickBrowseModMountPath()
        {
            OpenFolderDialog folderDialog = new OpenFolderDialog
            {
                Title = "Select Mod Directory",
                Directory = "."
            };

            string path = await folderDialog.ShowAsync(MainWindow);
            
            if(!string.IsNullOrWhiteSpace(path))
            {
                CurrentProfile.Mod.Mount.Path = path;
            } 

        }

        public void OnClickProfileConfig()
        {
            ProfileConfigWindow modOptions = new ProfileConfigWindow();
            modOptions.DataContext = this;
            
            modOptions.ShowDialog(MainWindow);
        }

        public void OnClickCreateProfile()
        {
            Profile profile = Profile.GetDefaultProfile();
            profile.Name = "New Profile";

            if (Profiles.Any(p => p.Name == profile.Name))
                return;
            
            Profiles.Add(profile);
            CurrentProfile = profile;
            
        }

        public async Task OnClickMountAdd()
        {
            AddMountDialog dialog = new AddMountDialog();

            Mount result = await dialog.ShowDialog<Mount>(MainWindow);

            if(result != null && !CurrentProfile.Mounts.ToList().Any(m => m.AppId == result.AppId || m.Path == result.Path) )
            {
                CurrentProfile.Mounts.Add(result);
            }
           
        }

        public void OnClickMountRemove()
        {
            if(CurrentMount != null && CurrentProfile.Mounts.Contains(CurrentMount))
            {
                CurrentProfile.Mounts.Remove(CurrentMount);
            }
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
