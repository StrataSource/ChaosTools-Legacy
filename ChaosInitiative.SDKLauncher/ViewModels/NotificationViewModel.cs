using System.Reactive;
using System.Reactive.Disposables;
using ReactiveUI;

namespace ChaosInitiative.SDKLauncher.ViewModels
{
    public class NotificationViewModel : ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; }
        
        public ReactiveCommand<Unit, Unit> OnClickClose { get; set; }

        public NotificationViewModel()
        {
            Activator = new ViewModelActivator();
            
            this.WhenActivated((CompositeDisposable disposable) =>
            {
                Disposable.Create(() =>
                {
                    
                }).DisposeWith(disposable);
            });
        }
    }
}