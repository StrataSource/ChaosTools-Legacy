using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SDKLauncher.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

    }
}
