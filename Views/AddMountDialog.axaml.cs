using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SDKLauncher.Models;

namespace SDKLauncher.Views
{
    class AddMountDialog : Window
    {

        public bool UseAppId { get; set; } = true;
        public Mount Mount { get; set; }

        public AddMountDialog()
        {
            AvaloniaXamlLoader.Load(this);

            Mount = new Mount();
            DataContext = this;
        }

        public void OnClickOk()
        {
            Close(Mount);
        }
    }
}
