using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ChaosInitiative.SDKLauncher.Util;
using ReactiveUI;
using Steamworks;

namespace ChaosInitiative.SDKLauncher.Models
{
    public class Mount : ReactiveObject
    {

        public Mount()
        {
            SelectedSearchPaths = new ObservableCollection<string>();
            PrimarySearchPath = string.Empty;
        }

        // -------------------------------------------------------------------------------------

        private string _mountPath;

        public string MountPath
        {
            get
            {
                if (AppId == 0) return _mountPath;
                try
                {
                    if (AppConfig.IsConfigSaved())
                    {
                        if (SteamApps.IsAppInstalled(AppId))
                        {
                            return SteamApps.AppInstallDir(AppId);
                        }
                    }
                } catch(NullReferenceException) {}

                return null;
            }
            set
            {
                AppId = 0;
                _mountPath = value;
                this.RaisePropertyChanged(nameof(AppId));
                this.RaisePropertyChanged(nameof(MountPath));
                this.RaisePropertyChanged(nameof(AvailableSearchPaths));
            }
        }
        private int _appId;
        public int AppId
        {
            get => _appId;
            set
            {
                _appId = value;
                this.RaisePropertyChanged(nameof(MountPath));
                this.RaisePropertyChanged(nameof(AvailableSearchPaths));
            }
        }

        public string PrimarySearchPath { get; set; }
        public bool IsRequired { get; set; }
        public ObservableCollection<string> SelectedSearchPaths { get; set; }

        public string BinDirectory
        {
            get
            {
                string binPath = $"{MountPath}/bin";
                string platformSpecificBinPath = $"{binPath}/{MountUtil.GetPlatformString()}";

                if (Directory.Exists(platformSpecificBinPath))
                {
                    binPath = platformSpecificBinPath;
                }

                return binPath;
            }
        }

        public string PrimarySearchPathDirectory => $"{MountPath}/{PrimarySearchPath}";

        // Don't you love it? :)
        public List<string> AvailableSearchPaths =>
            string.IsNullOrWhiteSpace(MountPath) ? new List<string>() :
                Directory.GetDirectories(MountPath)                                                    // Get all subdirectories
                    .Where(MountUtil.IsValidSearchPath)                                                // That have a gameinfo
                    .Where(d => !SelectedSearchPaths.Contains(Path.GetDirectoryName(d) ?? ""))       // Which are not already
                    .Select(d => d.Split(Path.DirectorySeparatorChar).Last())
                    .ToList();

        public bool Equals(Mount other)
        {
            if (other is null)
                return false;

            return _mountPath == other._mountPath &&
                   _appId == other._appId &&
                   PrimarySearchPath == other.PrimarySearchPath &&
                   IsRequired == other.IsRequired &&
                   SelectedSearchPaths.SequenceEqual(other.SelectedSearchPaths);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Mount);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_mountPath, _appId, PrimarySearchPath, IsRequired, SelectedSearchPaths);
        }
    }
}