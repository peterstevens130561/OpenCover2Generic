using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace BHGE.SonarQube.OpenCover2Generic.Consumer
{
    [Serializable]
    public class JobTimeOutException : ApplicationException
    {
        public JobTimeOutException(string message) : base(message)
        {
        }

        #region Serializable
        protected JobTimeOutException(SerializationInfo info, StreamingContext context)
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
