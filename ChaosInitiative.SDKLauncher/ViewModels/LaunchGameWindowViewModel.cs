using System.Reactive;
using System.Reactive.Disposables;
using ReactiveUI;

namespace ChaosInitiative.SDKLauncher.ViewModels
{
    public class LaunchGameWindowViewModel : ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; }
        
        public ReactiveCommand<Unit, Unit> OnClickLaunchGame { get; set; }

        public LaunchGameWindowViewModel()
        {
            Activator = new ViewModelActivator();

            this.WhenActivated(disposable => {Disposable.Create(() => {}).DisposeWith(disposable);});

            OnClickLaunchGame = ReactiveCommand.Create(() => {});
        }
    }
}