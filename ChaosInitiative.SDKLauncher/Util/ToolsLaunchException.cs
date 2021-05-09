using System;

namespace ChaosInitiative.SDKLauncher.Util
{
    public class ToolsLaunchException : Exception
    {
        public ToolsLaunchException(string message)
        {
            Message = message;
        }

        public override string Message { get; }
    }
}