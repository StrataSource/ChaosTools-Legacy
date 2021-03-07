using System;

namespace ChaosInitiative.SDKLauncher.Util
{
    public class ToolsLaunchException : Exception
    {

        private string _message;
        
        public ToolsLaunchException(string message)
        {
            _message = message;
        }

        public override string Message => _message;
    }
}