using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ChaosInitiative.SDKLauncher.Views.UserControls
{
    public class EditableListBox : UserControl
    {
        public EditableListBox()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}