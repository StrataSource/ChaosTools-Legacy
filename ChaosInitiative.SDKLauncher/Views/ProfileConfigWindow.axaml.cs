using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ChaosInitiative.SDKLauncher.Util;
using ChaosInitiative.SDKLauncher.ViewModels;
using ReactiveUI;

namespace ChaosInitiative.SDKLauncher.Views
{
    public class ProfileConfigWindow : ReactiveWindow<ProfileConfigViewModel>
    {

        protected Button CloseButton => this.FindControl<Button>("CloseButton");
        protected Button BrowseMountPathButton => this.FindControl<Button>("BrowseMountPathButton");

        public ProfileConfigWindow()
        {
            AvaloniaXamlLoader.Load(this);

            this.WhenActivated((CompositeDisposable disposable) =>
            {
                ViewModel.OnClickClose = ReactiveCommand.Create(Close);
                this.BindCommand(ViewModel, 
                                 vm => vm.OnClickClose, 
                                 v => v.CloseButton);

                ViewModel.OnClickBrowseMountPath = ReactiveCommand.CreateFromTask(OnClickBrowseModMountPath);
                this.BindCommand(ViewModel,
                                 vm => vm.OnClickBrowseMountPath, 
                                 v => v.BrowseMountPathButton);
            });
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

            var results = await folderDialog.ShowAsync(this);
            string path = Path.GetDirectoryName(results[0]); // We only accept 1 element, so just take the first
            
            if(path is not null && Directory.Exists(path))
            {
                var parent = Directory.GetParent(path);
                string mountPath = parent.FullName;
                string primarySearchPath = path.Substring(mountPath.Length);
                primarySearchPath = primarySearchPath.Replace("/", "")
                                           .Replace(@"\", "");
                
                ViewModel.Profile.Mod.Mount.MountPath = mountPath;
                ViewModel.Profile.Mod.Mount.PrimarySearchPath = primarySearchPath;
            } 

        }
    }
}
