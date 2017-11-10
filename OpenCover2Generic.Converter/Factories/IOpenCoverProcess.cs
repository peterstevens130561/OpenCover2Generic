using BHGE.SonarQube.OpenCover2Generic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Factories
{
    public interface IOpenCoverProcess : IProcess
    {
        bool RecoverableError { get;}
        bool Started { get; }
    }
}
