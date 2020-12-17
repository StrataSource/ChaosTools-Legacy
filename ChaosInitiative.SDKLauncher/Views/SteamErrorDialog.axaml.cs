using System.ComponentModel;
using Avalonia.Markup.Xaml;
using SDKLauncher.Util;

namespace SDKLauncher.Views
{
    public class SteamErrorDialog : BaseWindow, INotifyPropertyChanged
    {
        
        public new event PropertyChangedEventHandler PropertyChanged;
        
        public string Message { get; set; }

        // There must be a better place to store this.
        // I'd rather read this off a file, but that's a bit elaborate for a simple error message
        private string MessageUnknown => "An unknown error occured when trying to load the Steam API.\n\n" +
                                         "Troubleshooting:\n" +
                                         "- Ensure Steam is installed and open\n" +
                                         "- Close other running Steam games\n" +
                                         $"- Confirm that {SteamHelper.ApiDependencyName} and steam_appid.txt are present in this application's folder";

        private string MessageNotRunning => "Steam must be running to use SDKLauncher.";
        private string MessageNoDll => "The steam api couldn't be loaded.\n" +
                                       $"Please confirm that {SteamHelper.ApiDependencyName} is present in the application's folder!";

        public SteamErrorDialog() : this(SteamExceptionType.Unknown)
        {
        }
        
        public SteamErrorDialog(SteamExceptionType exceptionType)
        {
            switch (exceptionType)
            {
                case SteamExceptionType.Unknown: 
                    Message = MessageUnknown;
                    break;
                case SteamExceptionType.NotRunning: 
                    Message = MessageNotRunning;
                    break;
                case SteamExceptionType.ApiNotFound: 
                    Message = MessageNoDll;
                    break;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
        }
        
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