using Avalonia.Markup.Xaml;

namespace SDKLauncher.Views
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
            
        }
    }
}