using BHGE.SonarQube.OpenCover2Generic.Model;
using System;

namespace BHGE.SonarQube.OpenCoverWrapper
{
    internal interface ITestRunner
    {
        void CreateJobs(string[] testAssemblies, int chunkSize);
        void CreateJobConsumers(int consumers, TimeSpan jobTimeOut);
        IJobs Jobs { get; }
        void Wait();
    }
}