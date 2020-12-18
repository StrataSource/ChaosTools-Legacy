using Avalonia.Markup.Xaml;
using SDKLauncher.Models;

namespace SDKLauncher.Views
{
    public class AddMountDialog : BaseWindow
    {

        public bool UseAppId { get; set; } = true;
        public Mount Mount { get; } = new ();

        public string SelectedNamespace { get; set; }
        
        protected override void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = this;
        }

        public void OnClickOk()
        {
            Close(Mount);
        }

        public void OnClickAddNamespace()
        {
            Mount.SelectedSearchPaths.Add("New Namespace");
        }

        public void OnClickNamespaceAdd()
        {
            if (!string.IsNullOrWhiteSpace(SelectedNamespace) && !Mount.SelectedSearchPaths.Contains(SelectedNamespace))
            {   
                Mount.SelectedSearchPaths.Add(SelectedNamespace);
            }

        }

        public void OnClickNamespaceRemove()
        {

        }

    }
}
