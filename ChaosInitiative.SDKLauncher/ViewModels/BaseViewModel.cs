using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;

namespace ChaosInitiative.SDKLauncher.ViewModels
{
    public abstract class BaseViewModel : ReactiveObject, INotifyPropertyChanged
    {

        protected IClassicDesktopStyleApplicationLifetime ApplicationLifetime =>
            (IClassicDesktopStyleApplicationLifetime) Application.Current.ApplicationLifetime;
        protected Window MainWindow => ApplicationLifetime.MainWindow;
        protected Window MyWindow => ApplicationLifetime.Windows.First(w => w.DataContext == this);
        
        public new event PropertyChangedEventHandler PropertyChanged;

        public void InvokePropertyChangedEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
