using System;
using BHGE.SonarQube.OpenCover2Generic.Seams;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCover
{
    public interface IOpenCoverProcess : IProcess
    {
        bool Started { get; }
        string TestResultsPath { get; }

        ProcessState State { get; }

        void SetTimeOut(TimeSpan timeSpan);
    }
}
