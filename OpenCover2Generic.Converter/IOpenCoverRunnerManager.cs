using System.Diagnostics;
using System.IO;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner
{
    public interface IOpenCoverRunnerManager
    {
        string TestResultsPath { get; }

        void Run(ProcessStartInfo startInfo, StreamWriter writer);
    }
}