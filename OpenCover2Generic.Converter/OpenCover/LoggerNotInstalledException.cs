using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCover
{
    public class LoggerNotInstalledException : ApplicationException
    {
        public LoggerNotInstalledException() : base(@"SonarQubeLogger not installed, please follow installation instructions")
        {
        }

        #region Serializable
        protected LoggerNotInstalledException(SerializationInfo info, StreamingContext context)
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
