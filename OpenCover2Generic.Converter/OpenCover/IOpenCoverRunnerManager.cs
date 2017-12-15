using System;
using System.Diagnostics;
using System.IO;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner
{
    public interface IOpenCoverRunnerManager
    {
        bool HasTests { get; set; }
        string TestResultsPath { get; }

        void Run(ProcessStartInfo startInfo, StreamWriter writer, string assemblies);

        void SetTimeOut(TimeSpan timeOut);
    }
}