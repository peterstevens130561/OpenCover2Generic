using System;
using System.Runtime.Serialization;

namespace BHGE.SonarQube.OpenCover2Generic.CoverageConverters.Exceptions
{
    [Serializable]
    public class CommandLineArgumentException : InvalidOperationException

    {
        public CommandLineArgumentException(string message) : base(message)
        {
        }

        protected CommandLineArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public new virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
