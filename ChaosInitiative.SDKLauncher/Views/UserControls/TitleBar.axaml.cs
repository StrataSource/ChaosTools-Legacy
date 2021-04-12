using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ChaosInitiative.SDKLauncher.Views.UserControls
{
    /*
     * Some code taken from https://github.com/FrankenApps/Avalonia-CustomTitleBarTemplate/blob/521366625bddf8c295ca9ee1c457201affc4d107/Views/CustomTitleBars/WindowsTitleBar.axaml.cs#L91
     * licensed under MIT
     */
    public class TitleBar : UserControl
    {

        private readonly Button _btnMinimize;
        private readonly Button _btnMaximize;
        private readonly Button _btnQuit;

        private Window HostWindow => (Window) VisualRoot;
        
        
        private bool _isPointerPressed = false;
        private PixelPoint _startPosition = new PixelPoint(0, 0);
        private Point _mouseOffsetToOrigin = new Point(0, 0);
        
        public TitleBar()
        {
            InitializeComponent();

            _btnMinimize = this.FindControl<Button>("BtnMinimize");
            _btnMaximize = this.FindControl<Button>("BtnMaximize");
            _btnQuit = this.FindControl<Button>("BtnQuit");

            _btnMinimize.Click += MinimizeWindow;
            _btnMaximize.Click += MaximizeWindow;
            _btnQuit.Click += CloseWindow;
            
            this.PointerPressed += BeginListenForDrag;
            this.PointerMoved += HandlePotentialDrag;
            this.PointerReleased += HandlePotentialDrop;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            HostWindow.Close();
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            if (HostWindow.WindowState == WindowState.Normal)
            {
                HostWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                HostWindow.WindowState = WindowState.Normal;
            }
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            HostWindow.WindowState = WindowState.Minimized;
        }
        
        
        
        private void HandlePotentialDrop(object sender, PointerReleasedEventArgs e)
        {
            var pos = e.GetPosition(this);
            _startPosition = new PixelPoint((int)(_startPosition.X + pos.X - _mouseOffsetToOrigin.X), (int)(_startPosition.Y + pos.Y - _mouseOffsetToOrigin.Y));
            ((Window)this.VisualRoot).Position = _startPosition;
            _isPointerPressed = false;
        }

        private void HandlePotentialDrag(object sender, PointerEventArgs e)
        {
            if (_isPointerPressed)
            {
                var pos = e.GetPosition(this);
                _startPosition = new PixelPoint((int)(_startPosition.X + pos.X - _mouseOffsetToOrigin.X), (int)(_startPosition.Y + pos.Y - _mouseOffsetToOrigin.Y));
                ((Window)this.VisualRoot).Position = _startPosition;
            }
        }

        private void BeginListenForDrag(object sender, PointerPressedEventArgs e)
        {
            _startPosition = ((Window)this.VisualRoot).Position;
            _mouseOffsetToOrigin = e.GetPosition(this);
            _isPointerPressed = true;
        }
        
    }
}