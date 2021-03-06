using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace ChaosInitiative.SDKLauncher.Views
{
    public class SteamErrorDialog : ReactiveWindow<SteamErrorDialog>
    {
        
        public SteamErrorDialog()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
    }
}