using System.Reactive;
using ReactiveUI;

namespace ChaosInitiative.SDKLauncher.ViewModels
{
    public class SteamErrorViewModel : ReactiveObject
    {
        public ReactiveCommand<Unit, Unit> OnClickClose { get; }
    }
}