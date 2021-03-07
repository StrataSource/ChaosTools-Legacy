using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ChaosInitiative.SDKLauncher.ViewModels;
using ReactiveUI;

namespace ChaosInitiative.SDKLauncher.Views
{
    public class NotificationDialog : ReactiveWindow<NotificationViewModel>
    {

        private Button OkButton => this.FindControl<Button>("OkButton");
        private TextBlock MessageBox => this.FindControl<TextBlock>("MessageBox");
        
        // Default constructor is used by designer
        public NotificationDialog() : this("No message")
        {
        }

        public NotificationDialog(string message)
        {
            AvaloniaXamlLoader.Load(this);
            this.AttachDevTools();

            ViewModel = new NotificationViewModel();

            this.WhenActivated(disposable =>
            {
                ViewModel.OnClickClose = ReactiveCommand.Create(Close).DisposeWith(disposable);

                this.BindCommand(ViewModel, 
                                 vm => vm.OnClickClose, 
                                 v => v.OkButton).DisposeWith(disposable);

                MessageBox.Text = message;
            });
        }
        
    }
}