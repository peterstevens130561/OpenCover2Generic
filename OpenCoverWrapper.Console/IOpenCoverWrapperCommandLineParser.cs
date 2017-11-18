﻿using System;

namespace BHGE.SonarQube.OpenCoverWrapper
{
    internal interface IOpenCoverWrapperCommandLineParser
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