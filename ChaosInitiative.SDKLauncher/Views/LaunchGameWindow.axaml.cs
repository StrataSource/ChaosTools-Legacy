using System;
using System.IO;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ChaosInitiative.SDKLauncher.Models;
using ChaosInitiative.SDKLauncher.Util;
using ChaosInitiative.SDKLauncher.ViewModels;
using ReactiveUI;

namespace ChaosInitiative.SDKLauncher.Views
{
    public class LaunchGameWindow : ReactiveWindow<LaunchGameWindowViewModel>
    {
        protected CheckBox ToolsModeCheckBox => this.FindControl<CheckBox>("ToolsMode");
        protected CheckBox ConsoleCheckBox => this.FindControl<CheckBox>("Console");
        protected CheckBox DeveloperCheckBox => this.FindControl<CheckBox>("DeveloperMode");
        protected CheckBox GraphicsApiCheckBox => this.FindControl<CheckBox>("GraphicsApi");
        protected CheckBox LegacyUiCheckBox => this.FindControl<CheckBox>("LegacyUi");
        private Button LaunchGameButton => this.FindControl<Button>("LaunchGameButton");
        
        private readonly Profile _currentProfile;

        // Apparently used by VS Designer
        public LaunchGameWindow() : this(Profile.GetDefaultProfile())
        {
        }
        
        public LaunchGameWindow(Profile currentProfile)
        {
            AvaloniaXamlLoader.Load(this);
            this.AttachDevTools();

            ViewModel = new LaunchGameWindowViewModel();
            
            this.WhenActivated(disposables =>
            {
                ViewModel.OnClickLaunchGame = ReactiveCommand.Create(LaunchGame);
                
                this.BindCommand(ViewModel, 
                    vm => vm.OnClickLaunchGame, 
                    v => v.LaunchGameButton).DisposeWith(disposables);
            });
            
            _currentProfile = currentProfile;
        }
        
        private void LaunchGame()
        {
            string binDir = _currentProfile.Mod.Mount.BinDirectory;
            string executableName = _currentProfile.Mod.ExecutableName;
            string gameRootPath = _currentProfile.Mod.Mount.MountPath;
            
            if (OperatingSystem.IsWindows())
            {
                executableName += ".exe";
            }
            else if (OperatingSystem.IsLinux())
            {
                executableName += ".sh";
            }

            string binPath = Path.Combine(binDir, executableName);

            // See if the binary is in bin folder
            if (!File.Exists(binPath))
            {
                // Not in a chaos game. Try to find it in game root.
                binPath = Path.Combine(gameRootPath, executableName);
                if (!File.Exists(binPath))
                {
                    var dialog = new NotificationDialog($"Unable to find game binary '{binPath}'");
                    dialog.ShowDialog(this);
                    return;
                }
            }

            var args = $"{_currentProfile.Mod.LaunchArguments} -game {_currentProfile.Mod.Mount.PrimarySearchPath}";
            if (!string.IsNullOrWhiteSpace(_currentProfile.AdditionalMount.MountPath))
            {
                args += $" -mountmod \"{Path.Combine(_currentProfile.AdditionalMount.MountPath, _currentProfile.AdditionalMount.PrimarySearchPath)}\"";
            }
            if (ToolsModeCheckBox.IsChecked != null && ToolsModeCheckBox.IsChecked.Value && !args.Contains("-tools"))
                args += " -tools";
            if (LegacyUiCheckBox.IsChecked != null && LegacyUiCheckBox.IsChecked.Value && !args.Contains("-legacyui"))
                args += " -legacyui";
            if (GraphicsApiCheckBox.IsChecked != null && GraphicsApiCheckBox.IsChecked.Value && !args.Contains("-shaderapi"))
                args += " -shaderapi shaderapidx11";
            if (ConsoleCheckBox.IsChecked != null && ConsoleCheckBox.IsChecked.Value && !args.Contains("-console"))
                args += " -console";
            if (DeveloperCheckBox.IsChecked != null && DeveloperCheckBox.IsChecked.Value && !args.Contains("-dev"))
                args += " -dev";

            try
            {
                ToolsUtil.LaunchTool(binDir, executableName, args, false, gameRootPath);
            }
            catch (ToolsLaunchException e)
            {
                var dialog = new NotificationDialog(e.Message);
                dialog.ShowDialog(this);
            }
            
            Close();
        }
    }
}