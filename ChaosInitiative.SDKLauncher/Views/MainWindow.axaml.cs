using System.ComponentModel;
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
                ViewModel.OnClickEditProfile = ReactiveCommand.Create(() =>
                {
                    ProfileConfigWindow profileConfigWindow = new ProfileConfigWindow
                    {
                        DataContext = new ProfileConfigViewModel(ViewModel.CurrentProfile)
                    };
                    profileConfigWindow.ShowDialog(this);
                });
                this.BindCommand(this.ViewModel,
                                 vm => vm.OnClickEditProfile,
                                 v => v.EditProfileButton);
            });
            
            this.Closing += OnClosing;
        }

        private void OnClosing(object? sender, CancelEventArgs e)
        {
            ViewModel.Config.Save();
        }
    }
}
