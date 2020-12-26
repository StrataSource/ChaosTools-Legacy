using Avalonia.Markup.Xaml;
using ChaosInitiative.SDKLauncher.Models;

namespace ChaosInitiative.SDKLauncher.Views
{
    // TODO: Put AddMountDialog into properly separated view & viewmodel
    public class AddMountDialog : BaseWindow
    {

        public bool UseAppId { get; set; } = true;
        public Mount Mount { get; } = new ();

        public string SelectedSearchPath { get; set; }
        
        protected override void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

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

    }
}
