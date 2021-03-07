using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ChaosInitiative.SDKLauncher.ViewModels;
using ReactiveUI;

namespace ChaosInitiative.SDKLauncher.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {

        protected Button EditProfileButton => this.FindControl<Button>("EditProfileButton");
        
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                ViewModel.OnClickEditProfile.Subscribe(_ => EditProfile());
                ViewModel.OnClickOpenHammer.Subscribe(_ => OpenHammer());
            });
            
            this.Closing += OnClosing;
        }

        private void OnClosing(object? sender, CancelEventArgs e)
        {
            ViewModel.Config.Save();
        }

        private void EditProfile()
        {
            ProfileConfigWindow profileConfigWindow = new ProfileConfigWindow
            {
                DataContext = new ProfileConfigViewModel(ViewModel.CurrentProfile)
            };
            profileConfigWindow.ShowDialog(this);
        }
        
        private void OpenHammer()
        {
            string binDir = ViewModel.CurrentProfile.Mod.Mount.BinDirectory;

            if (string.IsNullOrWhiteSpace(binDir))
                return;

            string hammerPath = Path.Combine(binDir, "hammer.exe");

            if (!File.Exists(binDir))
            {
                
            }
            
            var hammerProcessStartInfo = new ProcessStartInfo
            {
                FileName = hammerPath,
                WorkingDirectory = binDir,
                Arguments = "-nosteam -nop4"
            };
            
            Process.Start(hammerProcessStartInfo);
        }
    }
}
