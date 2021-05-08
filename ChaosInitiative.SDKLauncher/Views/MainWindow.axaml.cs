using System;
using System.ComponentModel;
using System.IO;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ChaosInitiative.SDKLauncher.Util;
using ChaosInitiative.SDKLauncher.ViewModels;
using ReactiveUI;
using Steamworks;

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
                var arguments = "";

                var additionalMount = ViewModel.CurrentProfile.AdditionalMount;
                
                if (additionalMount is not null &&
                    !string.IsNullOrWhiteSpace(additionalMount.PrimarySearchPathDirectory))
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
                // This sets up all the correct paths to use to launch the tools and game
                // Note: This looks like it won't work here, but it really does, I promise
                //       Moving it outside this function crashes the application on load
                InitializeSteamClient((uint)ViewModel.CurrentProfile.Mod.Mount.AppId);
                
                ViewModel.OnClickEditProfile.Subscribe(_ => EditProfile()).DisposeWith(disposables);
                ViewModel.OnClickOpenHammer.Subscribe(_ => 
                    LaunchTool(
                        "hammer",
                        Tools.Hammer
                    )
                ).DisposeWith(disposables);
                ViewModel.OnClickOpenModelViewer.Subscribe(_ =>
                    LaunchTool(
                        "hlmv",
                        Tools.ModelViewer,
                        true,
                        ViewModel.CurrentProfile.Mod.Mount.MountPath
                    )
                ).DisposeWith(disposables);
                ViewModel.ShowNotification = ReactiveCommand.Create<string>(ShowNotification).DisposeWith(disposables);

                ViewModel.OnClickLaunchGame.Subscribe(_ => LaunchGame());
            });

            Closing += OnClosing;
        }

        private void LaunchTool(string executableName, Tools tool, bool windowsOnly = false, string workingDir = null, string binDir = null)
        {
            binDir ??= ViewModel.CurrentProfile.Mod.Mount.BinDirectory;
            workingDir ??= binDir;

            string args = tool switch
            {
                Tools.Hammer => HammerArguments,
                Tools.ModelViewer => $"-game {ViewModel.CurrentProfile.Mod.Mount.PrimarySearchPath}",
                _ => ""
            };

            try
            {
                ToolsUtil.LaunchTool(binDir, executableName, args, windowsOnly, workingDir);
            }
            catch (ToolsLaunchException e)
            {
                ShowNotification(e.Message);
            }
        }

        private void ShowNotification(string message)
        {
            var dialog = new NotificationDialog(message);
            dialog.ShowDialog(this);
        }

        private void LaunchGame()
        {
            var launchGame = new LaunchGameWindow(ViewModel.CurrentProfile);
            launchGame.ShowDialog(this);
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            ViewModel.Config.Save();
        }

        private void EditProfile()
        {
            var profileConfigWindow = new ProfileConfigWindow
            {
                DataContext = new ProfileConfigViewModel(ViewModel.CurrentProfile)
            };
            profileConfigWindow.ShowDialog(this);
        }

        private static void InitializeSteamClient(uint appId)
        {
            if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            {
                throw new Exception("Wrong application lifetime, contact a developer");
            }

            try
            {
                SteamClient.Init(appId);
            }
            catch (Exception e)
            {
                if (!e.Message.Contains("Steam"))
                    throw;

                // TODO: This doesn't work well with i3wm
                desktop.MainWindow = new NotificationDialog("Steam error. Please check that steam is running, and you own the intended app.");
                Directory.CreateDirectory("logs");
                File.WriteAllText($"logs/steam_error_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.log", e.Message);
            }
        }

        private enum Tools
        {
            Hammer,
            ModelViewer
        }
    }
}