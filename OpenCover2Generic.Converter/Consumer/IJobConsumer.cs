using BHGE.SonarQube.OpenCover2Generic.Model;
using System.Collections.Concurrent;

namespace BHGE.SonarQube.OpenCover2Generic.Consumer
{
    public interface IJobConsumer
    {
        /// <summary>
        /// keep on consuming jobs from jobs until no more left
        /// </summary>
        /// <param name="jobs"></param>
        void ConsumeJobs(IJobs jobs);
    }
}