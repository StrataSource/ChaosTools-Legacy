using System.ComponentModel;
using ReactiveUI;

namespace SDKLauncher.ViewModels
{
    public class ViewModelBase : ReactiveObject, INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler PropertyChanged;

        public void InvokePropertyChangedEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
