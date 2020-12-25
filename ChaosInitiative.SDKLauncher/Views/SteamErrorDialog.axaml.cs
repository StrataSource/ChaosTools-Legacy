using Avalonia.Markup.Xaml;

namespace ChaosInitiative.SDKLauncher.Views
{
    public class SteamErrorDialog : BaseWindow
    {
        
        protected override void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        public void OnClickClose()
        {
            Close();
        }
    }
}