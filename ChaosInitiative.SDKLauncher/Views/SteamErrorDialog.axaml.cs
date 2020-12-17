using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SDKLauncher.Views
{
    public class SteamErrorDialog : Window
    {
        
        public SteamErrorDialog()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }
        
        protected void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        public void OnClickClose()
        {
            
        }
    }
}