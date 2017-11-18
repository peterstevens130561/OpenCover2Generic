using BHGE.SonarQube.OpenCover2Generic.Model;
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
    public class JobTimeOutException : ApplicationException
    {
        public JobTimeOutException(string message) : base(message)
        {
        }

        public JobTimeOutException(IJob Job) : base($"Time out occurred in executing tests on {Job.Assemblies}")
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
                throw new ArgumentNullException(nameof(info));
            base.GetObjectData(info, context);
        }
        #endregion
    }
}
