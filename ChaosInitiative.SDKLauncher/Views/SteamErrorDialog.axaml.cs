using System.ComponentModel;
using Avalonia.Markup.Xaml;

namespace SDKLauncher.Views
{
    public class SteamErrorDialog : BaseWindow, INotifyPropertyChanged
    {
        
        public new event PropertyChangedEventHandler PropertyChanged;
        
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