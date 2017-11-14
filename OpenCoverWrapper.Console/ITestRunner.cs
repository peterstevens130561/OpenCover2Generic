using BHGE.SonarQube.OpenCover2Generic.Model;
using System.Collections.Concurrent;

namespace BHGE.SonarQube.OpenCoverWrapper
{
    internal interface ITestRunner
    {
        void CreateJobs(string[] testAssemblies, int chunkSize);
        void CreateJobConsumers(int consumers);
        BlockingCollection<IJob> Jobs { get; }
    }
}