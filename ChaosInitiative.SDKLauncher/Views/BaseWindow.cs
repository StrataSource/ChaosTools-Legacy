using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SDKLauncher.Views
{
    public class BaseWindow : Window
    {
        protected BaseWindow()
        {
            FinalizeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        protected virtual void InitializeComponent()
        {
        }

        private void FinalizeComponent()
        {
            InitializeComponent();
            AvaloniaXamlLoader.Load(this);
        }
    }
}