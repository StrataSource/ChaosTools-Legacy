using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SDKLauncher.ViewModels;
using System.ComponentModel;

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
            Closing += EventWindowClosing;
        }

        void EventWindowClosing(object sender, CancelEventArgs e)
        {
            // Accessing the viewmodel from here. Janky hack mate
            MainWindowViewModel vm = DataContext as MainWindowViewModel;
            vm?.Config.Save();
        }
    }
}
