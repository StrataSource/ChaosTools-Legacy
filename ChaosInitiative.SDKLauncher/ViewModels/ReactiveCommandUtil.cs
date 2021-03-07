using System.Reactive;
using ReactiveUI;

namespace ChaosInitiative.SDKLauncher.ViewModels
{
    public static class ReactiveCommandUtil
    {
        public static ReactiveCommand<Unit, Unit> CreateEmpty()
        {
            return ReactiveCommand.Create(() => { });
        }
    }
}