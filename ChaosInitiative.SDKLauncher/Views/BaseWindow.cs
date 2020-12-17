using Avalonia;
using Avalonia.Controls;

namespace SDKLauncher.Views
{
    public abstract class BaseWindow : Window
    {
        protected BaseWindow()
        {
            FinalizeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        protected abstract void InitializeComponent();

        private void FinalizeComponent()
        {
            InitializeComponent();
        }
    }
}