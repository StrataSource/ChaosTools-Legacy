using Avalonia.Markup.Xaml;

namespace SDKLauncher.Views
{
    public class MainWindow : BaseWindow
    {
        protected override void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
