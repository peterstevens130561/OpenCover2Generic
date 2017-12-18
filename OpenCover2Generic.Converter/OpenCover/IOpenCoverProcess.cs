using System;
using BHGE.SonarQube.OpenCover2Generic.Adapters;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCover
{
    public interface IOpenCoverProcess : IProcessAdapter
    {
        bool Started { get; }
        string TestResultsPath { get; }

        ProcessState State { get; }

        void SetTimeOut(TimeSpan timeSpan);
    }
}
