using System;
using System.ComponentModel;
using System.IO;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ChaosInitiative.SDKLauncher.Util;
using ChaosInitiative.SDKLauncher.ViewModels;
using ReactiveUI;

namespace ChaosInitiative.SDKLauncher.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {

        protected Button EditProfileButton => this.FindControl<Button>("EditProfileButton");
        protected Button OpenToolsModeButton => this.FindControl<Button>("OpenToolsModeButton");
        protected Button OpenGameButton => this.FindControl<Button>("OpenGameButton");

        private string HammerArguments
        {
            get
            {
                string arguments = "";

                var additionalMount = ViewModel.CurrentProfile.AdditionalMount;

                if (additionalMount is not null && 
                    !string.IsNullOrWhiteSpace(additionalMount.BinDirectory))
                {
                    arguments = $"-mountmod \"{additionalMount.PrimarySearchPathDirectory}\"";
                }
                else
                {
                    arguments = "-nop4"; // Chaos doesn't need these, but valve games do
                }

                return arguments;
            }
        } 
        
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
            this.AttachDevTools();

            this.WhenActivated(disposables =>
            {
                ViewModel.OnClickEditProfile.Subscribe(_ => EditProfile()).DisposeWith(disposables);
                ViewModel.OnClickOpenHammer.Subscribe(_ => LaunchTool("hammer", 
                                                                      HammerArguments, 
                                                                      true))
                         .DisposeWith(disposables);
                ViewModel.OnClickOpenModelViewer.Subscribe(_ => 
                                                               LaunchTool("hlmv", 
                                                                          $"-game {ViewModel.CurrentProfile.Mod.Mount.PrimarySearchPath}", 
                                                                          true, 
                                                                          ViewModel.CurrentProfile.Mod.Mount.MountPath))
                         .DisposeWith(disposables);
                ViewModel.ShowNotification = ReactiveCommand.Create<string>(ShowNotification).DisposeWith(disposables);

                ViewModel.OnClickLaunchGame.Subscribe(_ => LaunchGame(false));
                ViewModel.OnClickLaunchToolsMode.Subscribe(_ => LaunchGame(true));

            });
            
            Closing += OnClosing;
        }

        private void LaunchTool(string executableName, string args = "", bool windowsOnly = false, string workingDir = null, string binDir = null)
        {
            binDir ??= ViewModel.CurrentProfile.Mod.Mount.BinDirectory;
            workingDir ??= binDir;
            
            try
            {
                var process = ToolsUtil.LaunchTool(binDir, executableName, args, windowsOnly, workingDir);
            }
            catch (ToolsLaunchException e)
            {
                ShowNotification(e.Message);
            }
        }

        private void LaunchGame(bool toolsMode)
        {
            string binDir = ViewModel.CurrentProfile.Mod.Mount.BinDirectory;
            string executableName = ViewModel.CurrentProfile.Mod.ExecutableName;
            string gameRootPath = ViewModel.CurrentProfile.Mod.Mount.MountPath;

            if (OperatingSystem.IsWindows())
            {
                executableName += ".exe";
            }

            string binPath = Path.Combine(binDir, executableName);
            
            // See if the binary is in bin folder
            if (!File.Exists(binPath))
            {
                // Not in a chaos game. Try to find it in game root. Yes copy paste code i know. It's minimal, nobody cares
                binPath = Path.Combine(gameRootPath, executableName);

                if (!File.Exists(binPath))
                {
                    // Can't find the game bruh
                    ShowNotification($"Unable to find game binary '{binPath}'");
                    return;
                }
            }
            
            string args = $"{ViewModel.CurrentProfile.Mod.LaunchArguments} -game {ViewModel.CurrentProfile.Mod.Mount.PrimarySearchPath}";
            if (toolsMode)
            {
                args += " -tools";
            }

            try
            {
                LaunchTool(executableName, 
                           args,
                           false,
                           gameRootPath,
                           binDir);
            } catch(Exception) { }
        }

        private void ShowNotification(string message)
        {
            NotificationDialog dialog = new NotificationDialog(message);
            dialog.ShowDialog(this);
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            ViewModel.Config.Save();
        }

        private void EditProfile()
        {
            ProfileConfigWindow profileConfigWindow = new ProfileConfigWindow
            {
                DataContext = new ProfileConfigViewModel(ViewModel.CurrentProfile)
            };
            profileConfigWindow.ShowDialog(this);
        }
        
        
    }
}