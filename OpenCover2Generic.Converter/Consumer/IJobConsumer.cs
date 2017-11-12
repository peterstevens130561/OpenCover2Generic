using System.Collections.Concurrent;

namespace BHGE.SonarQube.OpenCover2Generic.Consumer
{
    public interface IJobConsumer
    {
        int Chunk { get; set; }

        /// <summary>
        /// keep on consuming jobs from jobs until no more left
        /// </summary>
        /// <param name="jobs"></param>
        void ConsumeJobs(BlockingCollection<string> jobs);
    }
}