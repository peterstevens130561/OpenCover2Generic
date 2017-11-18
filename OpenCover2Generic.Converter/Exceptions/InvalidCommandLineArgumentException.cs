using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Exceptions
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
