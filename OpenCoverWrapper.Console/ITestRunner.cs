using BHGE.SonarQube.OpenCover2Generic.Model;
using System.Collections.Concurrent;
using System;

namespace BHGE.SonarQube.OpenCoverWrapper
{
    internal interface ITestRunner
    {
        void CreateJobs(string[] testAssemblies, int chunkSize);
        void CreateJobConsumers(int consumers);
        IJobs Jobs { get; }
        bool HadJobTimeOut { get; }

        TimeSpan JobTimeOut { get; set; }
        void Wait();
    }
}