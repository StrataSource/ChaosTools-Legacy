using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ChaosInitiative.SDKLauncher.Models;
using ChaosInitiative.SDKLauncher.ViewModels;

namespace ChaosInitiative.SDKLauncher.Views
{
    public class AddMountDialog : ReactiveWindow<AddMountViewModel>
    {
        
        public AddMountDialog()
        {
            AvaloniaXamlLoader.Load(this);
        }
        /*
        public void OnClickOk()
        {
            Close(Mount);
        }

        public void OnClickAdd()
        {
            if (!string.IsNullOrWhiteSpace(SelectedSearchPath) && !Mount.SelectedSearchPaths.Contains(SelectedSearchPath))
            {   
                Mount.SelectedSearchPaths.Add(SelectedSearchPath);
            }

        }

        public void OnClickRemove()
        {

        }
*/
    }
}
