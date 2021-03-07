using System.Reactive;
using ReactiveUI;

namespace ChaosInitiative.SDKLauncher.ViewModels
{
    public class NotificationViewModel : ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; }
        
        public ReactiveCommand<Unit, Unit> OnClickClose { get; set; }
        public string Message { get; set; }

        public NotificationViewModel() : this("Default message")
        {
        }
        
        public NotificationViewModel(string message)
        {
            Activator = new ViewModelActivator();

            Message = message;
        }
    }
}