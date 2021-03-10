using System;
using System.ComponentModel;
using System.IO;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ChaosInitiative.SDKLauncher.Models;
using ChaosInitiative.SDKLauncher.Util;
using ChaosInitiative.SDKLauncher.ViewModels;
using MessageBox.Avalonia;
using ReactiveUI;

namespace ChaosInitiative.SDKLauncher.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {

        protected Button EditProfileButton => this.FindControl<Button>("EditProfileButton");
        protected Button OpenToolsModeButton => this.FindControl<Button>("OpenToolsModeButton");
        protected Button OpenGameButton => this.FindControl<Button>("OpenGameButton");

        private Profile CurrentProfile => ViewModel.CurrentProfile;

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
                                                                          $"-game {CurrentProfile.Mod.Mount.PrimarySearchPath}", 
                                                                          true, 
                                                                          CurrentProfile.Mod.Mount.MountPath))
                         .DisposeWith(disposables);

                ViewModel.OnClickLaunchGame.Subscribe(_ => LaunchGame(false));
                ViewModel.OnClickLaunchToolsMode.Subscribe(_ => LaunchGame(true));

            });
            
            Closing += OnClosing;
        }

        private void LaunchTool(string executableName, string args = "", bool windowsOnly = false, string workingDir = null, string binDir = null, bool allowProton = true)
        {
            binDir ??= ViewModel.CurrentProfile.Mod.Mount.BinDirectory;
            workingDir ??= binDir;

            if (windowsOnly && allowProton && OperatingSystem.IsLinux())
            {
                binDir = binDir.Replace("linux64", "win64");
                workingDir = binDir;
            }
            
            try
            {
                var process = ToolsUtil.LaunchTool(binDir, executableName, args, windowsOnly, workingDir);
            }
            catch (ToolsLaunchException e)
            {
                MessageBoxManager.GetMessageBoxStandardWindow("Failed to launch tool", e.Message).Show();
            }
        }

        private void LaunchGame(bool toolsMode)
        {
            string binDir = CurrentProfile.Mod.Mount.BinDirectory;
            string executableName = CurrentProfile.Mod.ExecutableName;
            string gameRootPath = CurrentProfile.Mod.Mount.MountPath;

            if (OperatingSystem.IsWindows())
            {
                executableName += ".exe";
            } 
            else if (OperatingSystem.IsLinux())
            {
                executableName += ".sh";
            }
            else if (OperatingSystem.IsLinux())
            {
                executableName += ".sh";
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
                    var msgBox = MessageBoxManager.GetMessageBoxStandardWindow("Failed to launch game", 
                                                                               $"Unable to find game binary '{binPath}'");
                    msgBox.ShowDialog(this);
                    return;
                }
            }
            
            string args = $"{CurrentProfile.Mod.LaunchArguments} -game {CurrentProfile.Mod.Mount.PrimarySearchPath}";
            if (toolsMode)
            {
                args += " -tools";
            }
            if (!string.IsNullOrWhiteSpace(CurrentProfile.AdditionalMount.MountPath))
            {
                args += $" -mountmod \"{Path.Combine(CurrentProfile.AdditionalMount.MountPath, CurrentProfile.AdditionalMount.PrimarySearchPath)}\"";
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

        private void OnClosing(object sender, CancelEventArgs e)
        {
            ViewModel.Config.Save();
        }

        private void EditProfile()
        {
            ProfileConfigWindow profileConfigWindow = new ProfileConfigWindow
            {
                DataContext = new ProfileConfigViewModel(CurrentProfile)
            };
            profileConfigWindow.ShowDialog(this);
        }
        
        
    }
}