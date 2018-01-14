using System;

namespace BHGE.SonarQube.OpenCoverWrapper
{
    public interface IOpenCoverWrapperCommandLineParser
    {
        string[] Args { get; set; }

        string GetTargetPath();
        string GetTargetArgs();
        string GetOpenCoverPath();
        string GetTestResultsPath();
        string[] GetTestAssemblies();
        int GetParallelJobs();
        TimeSpan GetJobTimeOut();
        int GetChunkSize();
        string GetOutputPath();
    }
}