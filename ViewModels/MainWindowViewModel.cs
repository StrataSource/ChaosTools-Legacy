using SDKLauncher.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace SDKLauncher.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Mod> Mods { get; set; }
        public Mod CurrentMod { get; set; }

        public AppConfig Config { get; set; }

        public MainWindowViewModel()
        {
            try
            {
                Config = AppConfig.LoadConfig();
            } catch (FileNotFoundException ex)
            {
                Config = AppConfig.CreateDefaultConfig();
                Config.Save();
            }

            Mods = new ObservableCollection<Mod>(Config.Mods);
            CurrentMod = Mods[0];
        }
    }
}
