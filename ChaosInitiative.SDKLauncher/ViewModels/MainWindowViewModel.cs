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
        public int CurrentProfileIndex { get; set; }
        
        public Profile CurrentProfile => Profiles[CurrentProfileIndex];
        
        [Reactive]
        public ObservableCollection<Profile> Profiles { get; set; }
        public AppConfig Config { get; set; }

        public ReactiveCommand<Unit, Unit> OnClickOpenHammer { get; }
        public ReactiveCommand<Unit, Unit> OnClickCreateMod { get; }
        public ReactiveCommand<Unit, Unit> OnClickEditProfile { get; }
        public ReactiveCommand<Unit, Unit> OnClickCreateProfile { get; }
        public ReactiveCommand<Unit, Unit> OnClickDeleteProfile { get; }
        
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
            
            OnClickCreateMod = ReactiveCommandUtil.CreateEmpty();
            OnClickOpenHammer = ReactiveCommand.Create(() => { }, Observable.Return(OperatingSystem.IsWindows()));

            OnClickCreateProfile = ReactiveCommand.Create(CreateProfile);
            OnClickDeleteProfile = ReactiveCommand.Create(DeleteProfile);
            OnClickEditProfile = ReactiveCommandUtil.CreateEmpty();
        }
        
        public void CreateProfile()
        {
            Profile profile = Profile.GetDefaultProfile();
            profile.Name = "New Profile";

            if (Profiles.Any(p => p.Name == profile.Name))
                return;
            
            Profiles.Add(profile);
            CurrentProfileIndex = Profiles.Count - 1;
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
