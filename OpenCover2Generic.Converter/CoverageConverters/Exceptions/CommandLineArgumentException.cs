using System;

namespace BHGE.SonarQube.OpenCover2Generic.Exceptions
{
    [Serializable]
    public class CommandLineArgumentException : InvalidOperationException

    {
        public CommandLineArgumentException(string message) : base(message)
        {
        }
    }
}
