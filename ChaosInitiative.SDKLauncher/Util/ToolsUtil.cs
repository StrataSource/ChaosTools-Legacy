using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Steamworks;
using Steamworks.Data;

namespace ChaosInitiative.SDKLauncher.Util
{
    public class ToolsUtil
    {
        public static AppId ProtonAppId = 1245040;
        public static string SteamPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/.local/share/Steam/";
        
        /// <summary>
        /// Launches an arbitrary tool
        /// </summary>
        /// <param name="binDirectory"></param>
        /// <param name="executableName"></param>
        /// <param name="args"></param>
        /// <param name="windowsOnly"></param>
        /// <param name="workingDir"></param>
        /// <param name="allowProton">If this is "Windows only", allow it to be launched with proton on Linux</param>
        /// <returns></returns>
        /// <exception cref="ToolsLaunchException"></exception>
        public static Process LaunchTool(string binDirectory, string executableName, string args = "", bool windowsOnly = false, string workingDir = null, bool allowProton = true)
        {
            if (windowsOnly && !OperatingSystem.IsWindows() && !allowProton)
            {
                throw new ToolsLaunchException("This tool is windows-only");
            }

            string extension = (OperatingSystem.IsWindows() || windowsOnly) && !executableName.EndsWith(".exe") ? ".exe" : "";
            string executablePath = $"{binDirectory}/{executableName}{extension}";

            if (!File.Exists(executablePath))
            {
                throw new ToolsLaunchException($"Unable to find tool binary '{executablePath}'");
            }

            if (windowsOnly && allowProton)
            {
                return LaunchToolWithProton(binDirectory, executableName, args, workingDir);
            }
            else
            {
                var hammerProcessStartInfo = new ProcessStartInfo
                {
                    FileName = executablePath,
                    WorkingDirectory = workingDir ?? binDirectory,
                    Arguments = args
                };
            
                return Process.Start(hammerProcessStartInfo);    
            }
        }

        /// <summary>
        /// Runs a windows-only tool with proton/wine on linux
        /// </summary>
        /// <param name="binDir">Directory where the executable is located</param>
        /// <param name="exe">Executable to launch</param>
        /// <param name="args">Argument string</param>
        /// <param name="workingDir">Working directory to launch the app from</param>
        /// <param name="appid">AppID to tell steam that we're launching with</param>
        /// <param name="use_wined3d">Use WineD3D instead of dxvk. Some apps break with dxvk (e.g. hammer)</param>
        /// <param name="prefix">Proton prefix to use, defaults to ~/.config/ChaosSDKLauncher if not specified</param>
        /// <returns></returns>
        public static Process LaunchToolWithProton(string binDir, string exe, string args = "",
            string workingDir = null, int appid = 0, bool use_wined3d = true, string prefix = null)
        {
            if (OperatingSystem.IsWindows())
            {
                throw new ToolsLaunchException("Tools cannot be launched with proton on Windows");
            }

            /* References for all this crapola: https://github.com/ChaosInitiative/ChaosTools/issues/12 */
            /* tldr; need to set a bunch of envvars to appease proton, and dxvk doesn't like mfc (or the other way around) */
            
            if (prefix == null)
            {
                prefix = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/ChaosSDKLauncher";
            }

            string protonPath = null;
            if (!FindOrCreateProtonPrefix(ref protonPath, prefix))
                throw new ToolsLaunchException("Failed to create the proton prefix");
            
            var protonStartInfo = new ProcessStartInfo();
            protonStartInfo.WorkingDirectory = workingDir ?? binDir;

            if(appid != 0)
                protonStartInfo.Environment.Add("SteamGameId", appid.ToString());
            
            protonStartInfo.Environment.Add("PROTON_LOG", "1");
            protonStartInfo.Environment.Add("STEAM_COMPAT_DATA_PATH", prefix);
            protonStartInfo.Environment.Add("STEAM_COMPAT_CLIENT_INSTALL_PATH", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/.steam/");
            
            if(use_wined3d)
                protonStartInfo.Environment.Add("PROTON_USE_WINED3D", "1");

            protonStartInfo.FileName = protonPath;
            protonStartInfo.Arguments = $"run \"{binDir}/{exe}.exe\" {args}";

            return Process.Start(protonStartInfo);
        }
        
        /// <summary>
        /// Finds or creates a proton prefix we can run tools from
        /// </summary>
        /// <param name="proton_path">Out parameter for the proton executable path</param>
        /// <param name="directory">Prefix directory</param>
        /// <returns></returns>
        /// <exception cref="ToolsLaunchException">Something failed</exception>
        public static bool FindOrCreateProtonPrefix(ref string proton_path, string directory)
        {
            /* Determine our proton install path */
            if (!SteamApps.IsAppInstalled(ProtonAppId))
                throw new ToolsLaunchException("Proton 5.13 is not installed");
            proton_path = SteamApps.AppInstallDir(ProtonAppId);
            proton_path += "/proton";
            Console.WriteLine(proton_path);

            /* Create the prefix if it doesn't actually exist */
            if (!File.Exists(directory + "/version"))
            {
                /* Make sure the directory actually exists first */
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (UnauthorizedAccessException)
                {
                    throw new ToolsLaunchException(
                        $"Failed to create proton 5.13 prefix, access to '{directory}' not permitted.");
                }
                catch (Exception)
                {
                    throw new ToolsLaunchException($"Failed to create proton 5.13 prefix at '{directory}");
                }

                /* To do this, let's just run proton with xcopy /?, as a dummy. proton will automatically create the pfx for us */
                var startInfo = new ProcessStartInfo();
                startInfo.FileName = proton_path;
                startInfo.Arguments = "run xcopy /?";
                
                Console.WriteLine(proton_path + " " + startInfo.Arguments);
                
                startInfo.Environment.Add("STEAM_COMPAT_DATA_PATH", directory);
                startInfo.Environment.Add("STEAM_COMPAT_CLIENT_INSTALL_PATH", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/.steam");
                
                var process = Process.Start(startInfo);

                /* Wait for 20 seconds. Seems a bit long, but crappy computers will probably take longer */
                process.WaitForExit(20000);

                if (!process.HasExited || process.ExitCode != 0)
                    throw new ToolsLaunchException($"Failed to create proton 5.13 prefix at '{directory}'");

                /* Copy in steam DLLs because proton will NOT do this for us :( */
                var steamDllPath = directory + "/pfx/drive_c/Program Files (x86)/Steam";
                try
                {
                    Console.WriteLine("Copying in Steam DLLs");
                    File.Copy(SteamPath + "steamclient.dll", steamDllPath + "/steamclient.dll", overwrite: true);
                    File.Copy(SteamPath + "steamclient64.dll", steamDllPath + "/steamclient64.dll", overwrite: true);
                    File.Copy(SteamPath + "legacycompat/Steam.dll", steamDllPath + "/Steam.dll", overwrite: true);
                }
                catch (Exception)
                {
                    throw new ToolsLaunchException($"Failed to copy Steam DLLs into prefix at {directory}");
                }
            }

            return true;
        }
    }
}