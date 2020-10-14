using SDKLauncher.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SDKLauncher.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Mod> Mods { get; set; }
        public Mod CurrentMod { get; set; }

        public MainWindowViewModel()
        {
            Mods = new ObservableCollection<Mod>
            {
                new Mod() {
                    Name = "Portal 2: Community Edition"
                },
                new Mod() {
                    Name = "Portal: Revolution"
                }
            };
            CurrentMod = Mods[0];
        }
    }
}
