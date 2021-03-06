using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ChaosInitiative.SDKLauncher.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ChaosInitiative.SDKLauncher.ViewModels
{
    public class MainWindowViewModel : ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; }

        [Reactive]
        public string TestString { get; set; } = "";
        
        [Reactive]
        public int CurrentProfileIndex { get; set; }
        
        public Profile CurrentProfile => Profiles[CurrentProfileIndex];
        
        [Reactive]
        public ObservableCollection<Profile> Profiles { get; set; }
        public AppConfig Config { get; set; }

        public ReactiveCommand<Unit, Unit> OnClickOpenHammer { get; set; }
        public ReactiveCommand<Unit, Unit> OnClickCreateMod { get; set; }
        public ReactiveCommand<Unit, Unit> OnClickEditProfile { get; set; }
        public ReactiveCommand<Unit, Unit> OnClickCreateProfile { get; set; }
        public ReactiveCommand<Unit, Unit> OnClickDeleteProfile { get; set; }
        
        public MainWindowViewModel()
        {

            Activator = new ViewModelActivator();
            
            this.WhenActivated((CompositeDisposable disposable) =>
            {
                Disposable.Create(() =>
                {
                    
                }).DisposeWith(disposable);
            });
            
            Config = AppConfig.LoadConfigOrCreateDefault();

            Profiles = new ObservableCollection<Profile>(Config.Profiles);
            CurrentProfileIndex = Config.DefaultProfileIndex;
            
            OnClickCreateMod = ReactiveCommand.Create(CreateMod);
            OnClickOpenHammer = ReactiveCommand.Create(OpenHammer, Observable.Return(OperatingSystem.IsWindows()));

            OnClickCreateProfile = ReactiveCommand.Create(CreateProfile);
            OnClickDeleteProfile = ReactiveCommand.Create(DeleteProfile);
        }

        public void OpenHammer()
        {
            string binDir = CurrentProfile.Mod.Mount.BinDirectory;

            if (string.IsNullOrWhiteSpace(binDir))
                return;

            string hammerPath = Path.Combine(binDir, "hammer.exe");
            
            var hammerProcessStartInfo = new ProcessStartInfo
            {
                FileName = hammerPath,
                WorkingDirectory = binDir
            };
            
            Process.Start(hammerProcessStartInfo);
        }

        public void CreateProfile()
        {
            Profile profile = Profile.GetDefaultProfile();
            profile.Name = "New Profile";

            if (Profiles.Any(p => p.Name == profile.Name))
                return;
            
            Profiles.Add(profile);
            CurrentProfileIndex = Config.DefaultProfileIndex;
        }

        public void DeleteProfile()
        {
            Profiles.RemoveAt(CurrentProfileIndex);
            CurrentProfileIndex = 0;
        }

        public void CreateMod()
        {
            
        }

    }
}
