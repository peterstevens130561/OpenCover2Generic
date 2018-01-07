using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace BHGE.SonarQube.OpenCover2Generic.CoverageConverters.Exceptions
{
    [Serializable]
    public class InvalidCommandLineArgumentException : ApplicationException
    {
        public InvalidCommandLineArgumentException(string message) : base(message)
        {
        }
        #region Serializable
        protected InvalidCommandLineArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context){
        }
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }
            base.GetObjectData(info, context);
        }
        #endregion
    }
}
