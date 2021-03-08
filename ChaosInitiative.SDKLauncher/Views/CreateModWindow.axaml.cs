using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ChaosInitiative.SDKLauncher.ViewModels;

namespace ChaosInitiative.SDKLauncher.Views
{
    public class CreateModWindow : ReactiveWindow<CreateModViewModel>
    {
        public CreateModWindow()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
