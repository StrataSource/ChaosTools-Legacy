using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ChaosInitiative.SDKLauncher.Models;
using ChaosInitiative.SDKLauncher.Util;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ChaosInitiative.SDKLauncher.ViewModels
{
    public class MainWindowViewModel : ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; }
        
        [Reactive]
        public bool ShowSaveConfirmation { get; set; }

        public int CurrentProfileIndex
        {
            get => Config.DefaultProfileIndex;
            set
            {
                Config.DefaultProfileIndex = value;
                this.RaisePropertyChanged(nameof(CurrentProfileIndex));
            }
        }

        public Profile CurrentProfile => Profiles[CurrentProfileIndex];
        
        [Reactive]
        public ObservableCollection<Profile> Profiles { get; set; }
        public AppConfig Config { get; set; }

        public ReactiveCommand<Unit, Unit> OnClickOpenHammer { get; }
        public ReactiveCommand<Unit, Unit> OnClickOpenModelViewer { get; }
        public ReactiveCommand<Unit, Unit> OnClickLaunchGame { get; }        
        public ReactiveCommand<Unit, Unit> OnClickLaunchToolsMode { get; }        
        public ReactiveCommand<Unit, Unit> OnClickCreateMod { get; }
        public ReactiveCommand<Unit, Unit> OnClickEditProfile { get; }
        public ReactiveCommand<Unit, Unit> OnClickCreateProfile { get; }
        public ReactiveCommand<Unit, Unit> OnClickDeleteProfile { get; }
        public ReactiveCommand<Unit, Unit> OnClickSaveConfig { get; }
        
        public MainWindowViewModel()
        {

            Activator = new ViewModelActivator();
            
            this.WhenActivated(disposable =>
            {
                Disposable.Create(() =>
                {
                    
                }).DisposeWith(disposable);
            });
            
            Config = AppConfig.LoadConfigOrCreateDefault();

            Profiles = new ObservableCollection<Profile>(Config.Profiles);
            Profiles.CollectionChanged += (sender, args) =>
            {
                Config.Profiles.AddRange( args.NewItems?.OfType<Profile>() ?? Array.Empty<Profile>() );
                Config.Profiles.RemoveMany( args.OldItems?.OfType<Profile>() ?? Array.Empty<Profile>() );
            };

            OnClickCreateMod = ReactiveCommandUtil.CreateEmpty();
            OnClickOpenHammer = ReactiveCommand.Create(() => { });
            OnClickOpenModelViewer = ReactiveCommand.Create(() => { });

            OnClickLaunchGame = ReactiveCommandUtil.CreateEmpty();
            OnClickLaunchToolsMode = ReactiveCommandUtil.CreateEmpty();
            
            OnClickCreateProfile = ReactiveCommand.Create(CreateProfile);
            OnClickDeleteProfile = ReactiveCommand.Create(DeleteProfile);
            OnClickEditProfile = ReactiveCommandUtil.CreateEmpty();

            OnClickSaveConfig = ReactiveCommand.CreateFromTask(async () =>
            {
                Config.Save();
                ShowSaveConfirmation = true;
                await Task.Delay(2000);
                ShowSaveConfirmation = false;
            });
        }

        public void CreateProfile()
        {
            Profile profile = Profile.GetDefaultProfile();
            
            string profileName = "";
            for (int i = 1; i <= 10; i++)
            {
                if (i == 10)
                    return;
                
                profileName = $"New Profile ({i})";
                if (Profiles.All(p => p.Name != profileName))
                    break;
            }

            profile.Name = profileName;
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
