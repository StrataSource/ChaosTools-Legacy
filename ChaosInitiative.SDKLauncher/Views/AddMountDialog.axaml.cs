using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SDKLauncher.Models;
using SDKLauncher.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace SDKLauncher.Views
{
    class AddMountDialog : Window
    {

        public bool UseAppId { get; set; } = true;
        public Mount Mount { get; set; }

        public string SelectedNamespace { get; set; }


        public AddMountDialog()
        {
            AvaloniaXamlLoader.Load(this);

            Mount = new Mount();
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
