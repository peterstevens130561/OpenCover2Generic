using BHGE.SonarQube.OpenCover2Generic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;

namespace BHGE.SonarQube.OpenCover2Generic.Factories
{
    public interface IOpenCoverProcess : IProcess
    {
        bool Started { get; }
        string TestResultsPath { get; }

        ProcessState State { get; }

        void SetTimeOut(TimeSpan timeSpan);
    }
}
