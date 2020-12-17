using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using SDKLauncher.Models;
using SDKLauncher.Views;

namespace SDKLauncher.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Profile> Profiles { get; set; }
        //public Profile CurrentProfile { get; set; }
        public int CurrentProfileIndex { get; set; }
        public Profile CurrentProfile => Profiles[CurrentProfileIndex];
        public Mount CurrentMount { get; set; }
        public AppConfig Config { get; set; }

        private Window MainWindow => ((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow;

        private readonly ProfileConfigWindow _modOptions;
        
        public MainWindowViewModel()
        {
            try
            {
                Config = AppConfig.LoadConfig();
            } 
            catch (FileNotFoundException)
            {
                Config = AppConfig.CreateDefaultConfig();
                Config.Save();
            }

            Profiles = new ObservableCollection<Profile>(Config.Profiles);
            CurrentProfileIndex = Config.DefaultProfileIndex;
            
            _modOptions = new ProfileConfigWindow();
            _modOptions.DataContext = this;
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
                //CurrentProfile.Mod.Mount.Path = path;
            } 

        }

        public void OnClickOpenProfileConfig()
        {
            _modOptions.Show();
        }

        public void OnClickCloseProfileConfig()
        {
            _modOptions.Close();
        }

        public void OnClickCreateProfile()
        {
            Profile profile = Profile.GetDefaultProfile();
            profile.Name = "New Profile";

            if (Profiles.Any(p => p.Name == profile.Name))
                return;
            
            Profiles.Add(profile);
            CurrentProfileIndex = Config.DefaultProfileIndex;
        }

        public async Task OnClickMountAdd()
        {
            AddMountDialog dialog = new AddMountDialog();

            Mount result = await dialog.ShowDialog<Mount>(MainWindow);

//            if(result != null && !CurrentProfile.Mounts.ToList().Any(m => m.AppId == result.AppId || m.Path == result.Path) )
//            {
//                CurrentProfile.Mounts.Add(result);
//            }
            if(result != null && !CurrentProfile.Mounts.ToList().Any(m => m.AppId == result.AppId || m.Path == result.Path) )
            {
                Profiles[CurrentProfileIndex].Mounts.Add(result);
            }
           
        }

        public void OnClickMountRemove()
        {
            if(CurrentMount != null && CurrentProfile.Mounts.Contains(CurrentMount))
            {
                CurrentProfile.Mounts.Remove(CurrentMount);
            }
        }

        public void OnClickSaveConfig()
        {
            Config.Save();
        }

        // ====================================

        private void LaunchTool(string executableName)
        {

            string extension;

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT: 
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
