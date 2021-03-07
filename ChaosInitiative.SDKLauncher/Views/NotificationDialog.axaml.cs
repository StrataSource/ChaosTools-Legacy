using System.Reactive.Disposables;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ChaosInitiative.SDKLauncher.ViewModels;
using ReactiveUI;

namespace ChaosInitiative.SDKLauncher.Views
{
    public class NotificationDialog : ReactiveWindow<NotificationViewModel>
    {
        
        // Default constructor is used by designer
        public NotificationDialog() : this("No message")
        {
        }

        public NotificationDialog(string message)
        {
            AvaloniaXamlLoader.Load(this);

            ViewModel = new NotificationViewModel(message);

            this.WhenActivated(disposable =>
            {
                ViewModel.OnClickClose = ReactiveCommand.Create(Close).DisposeWith(disposable);
            });
        }
        
    }
}