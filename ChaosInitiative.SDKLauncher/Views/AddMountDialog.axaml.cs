using SDKLauncher.Models;

namespace SDKLauncher.Views
{
    class AddMountDialog : BaseWindow
    {

        public bool UseAppId { get; set; } = true;
        public Mount Mount { get; } = new ();

        public string SelectedNamespace { get; set; }


        public AddMountDialog()
        {
            DataContext = this;
        }

        public void OnClickOk()
        {
            Close(Mount);
        }

        public void OnClickAddNamespace()
        {
            Mount.Namespaces.Add("New Namespace");
        }

        public void OnClickNamespaceAdd()
        {
            if (!string.IsNullOrWhiteSpace(SelectedNamespace) && !Mount.Namespaces.Contains(SelectedNamespace))
            {   
                Mount.Namespaces.Add(SelectedNamespace);
            }

        }

        public void OnClickNamespaceRemove()
        {

        }

    }
}
