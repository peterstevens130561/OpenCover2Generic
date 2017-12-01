using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCover
{
    public enum ProcessState
    {
        None,
        Busy,
        RecoverableFailure,
        IrrecoverableFailure,
        NoResults,
        TimedOut,
        Done
    }
}
