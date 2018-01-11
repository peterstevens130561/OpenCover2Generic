using System;
using BHGE.SonarQube.OpenCover2Generic.DomainModel;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.RunTests
{
    public interface ITestRunner
    {
        void CreateJobs(string[] testAssemblies, int chunkSize);
        void CreateJobConsumers(int consumers, TimeSpan jobTimeOut);
        IJobs Jobs { get; }
        void Wait();
    }
}