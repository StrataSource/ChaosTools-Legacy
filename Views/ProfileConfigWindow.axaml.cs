using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SDKLauncher.Views
{
    class ProfileConfigWindow : Window
    {

        public ProfileConfigWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

    }
}
