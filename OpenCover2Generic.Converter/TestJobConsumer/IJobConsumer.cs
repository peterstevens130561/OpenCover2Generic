using System;
using BHGE.SonarQube.OpenCover2Generic.DomainModel;

namespace BHGE.SonarQube.OpenCover2Generic.TestJobConsumer
{
    public interface IJobConsumer
    {
        /// <summary>
        /// keep on consuming jobs from jobs until no more left
        /// </summary>
        /// <param name="jobs"></param>
        /// <param name="jobTimeOut">timeout</param>
        void ConsumeTestJobs(IJobs jobs, TimeSpan jobTimeOut);
    }
}