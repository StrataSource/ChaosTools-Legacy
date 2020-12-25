using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using ChaosInitiative.SDKLauncher.Models;
using ChaosInitiative.SDKLauncher.Util;
using ChaosInitiative.SDKLauncher.Views;

namespace ChaosInitiative.SDKLauncher.ViewModels
{
    public class ProfileConfigViewModel : BaseViewModel
    {
        public Profile Profile { get; set; }
        public Mount SelectedMount { get; set; }

        public ProfileConfigViewModel() : this(Profile.GetDefaultProfile()) // Constructor for demonstration
        {
        }
        
        public ProfileConfigViewModel(Profile profile)
        {
            Profile = profile;
        }
        
        public async Task OnClickBrowseModMountPath()
        {
            OpenFileDialog folderDialog = new OpenFileDialog
            {
                Title = "Select gameinfo.txt",
                Directory = ".",
                Filters = new List<FileDialogFilter>
                {
                    MountUtil.GameInfoFileFilter
                },
                AllowMultiple = false
            };

            var results = await folderDialog.ShowAsync(MyWindow);
            string path = Path.GetDirectoryName(results[0]); // We only accept 1 element, so just take the first
            
            if(Directory.Exists(path))
            {
                Profile.Mod.Mount.MountPath = path;
            } 

        }
        
        public async Task OnClickMountAdd()
        {
            AddMountDialog dialog = new AddMountDialog();

            Mount result = await dialog.ShowDialog<Mount>(MyWindow);

            if(result != null && !Profile.Mounts.ToList().Any(m => m.AppId == result.AppId || m.MountPath == result.MountPath) )
            {
                Profile.Mounts.Add(result);
            }
           
        }

        public void OnClickMountRemove()
        {
            if(SelectedMount != null && Profile.Mounts.Contains(SelectedMount))
            {
                Profile.Mounts.Remove(SelectedMount);
            }
        }

        public void OnClickClose()
        {
            MyWindow.Close();
        }
        
    }
}