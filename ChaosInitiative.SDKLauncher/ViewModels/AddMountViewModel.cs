using System.Reactive;
using ChaosInitiative.SDKLauncher.Models;
using ReactiveUI;

namespace ChaosInitiative.SDKLauncher.ViewModels
{
    public class AddMountViewModel : ReactiveObject
    {
        public AddMountViewModel(ReactiveCommand<Unit, Unit> onClickAdd)
        {
            OnClickAdd = onClickAdd;
        }

        public bool UseAppId { get; set; } = true;
        public Mount Mount { get; } = new ();

        public string SelectedSearchPath { get; set; }
        
        public ReactiveCommand<Unit, Unit> OnClickAdd { get; }
        
    }
}