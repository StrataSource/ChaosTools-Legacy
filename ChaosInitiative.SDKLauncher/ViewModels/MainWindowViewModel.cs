using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ChaosInitiative.SDKLauncher.Models;
using ChaosInitiative.SDKLauncher.Util;
using ChaosInitiative.SDKLauncher.Views;

namespace ChaosInitiative.SDKLauncher.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollection<Profile> Profiles { get; set; }
        public int CurrentProfileIndex { get; set; }
        public Profile CurrentProfile => Profiles[CurrentProfileIndex];
        public AppConfig Config { get; set; }

        public MainWindowViewModel()
        {
            Config = AppConfig.LoadConfigOrCreateDefault();

            Profiles = new ObservableCollection<Profile>(Config.Profiles);
            CurrentProfileIndex = Config.DefaultProfileIndex;
        }
        
        public void OnClickHammer()
        {
            // TODO: Make it so this doesn't use the first item
            ToolUtil.LaunchTool("hammer", $"-mountmod=\"{CurrentProfile.Mounts[0].PrimarySearchPathDirectory}\"");
        }
        
        public void OnClickCreateMod()
        {
            CreateModWindow modOptions = new CreateModWindow();

            modOptions.ShowDialog(MainWindow);
        }
        
        public void OnClickOpenProfileConfig()
        {
            ProfileConfigWindow profileConfigWindow = new ProfileConfigWindow
            {
                DataContext = new ProfileConfigViewModel(CurrentProfile)
            };
            profileConfigWindow.ShowDialog(MainWindow);
        }
        
        public void OnClickCreateProfile()
        {
            Profile profile = Profile.GetDefaultProfile();
            profile.Name = "New Profile";

            if (Profiles.Any(p => p.Name == profile.Name))
                return;
            
            Profiles.Add(profile);
            CurrentProfileIndex = Config.DefaultProfileIndex;
            InvokePropertyChangedEvent(nameof(CurrentProfileIndex));
        }

        public void OnClickSaveConfig()
        {
            Config.Save();
        }

    }
}
