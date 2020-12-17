using System;

namespace SDKLauncher.Steam
{
    public class SteamException : Exception
    {
        
        public SteamExceptionType Type { get; set; }
        
        public SteamException(SteamExceptionType type, string message = "") : base(message)
        {
            Type = type;
        }
    }

    public enum SteamExceptionType
    {
        Unknown,
        NotRunning,
        ApiNotFound
    }
}