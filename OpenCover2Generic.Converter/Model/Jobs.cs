using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public class Jobs : IJobs
    {
        private readonly BlockingCollection<IJob> _jobs = new BlockingCollection<IJob>();

        public void Add(IJob job)
        {
            _jobs.Add(job);
        }

        public void CompleteAdding()
        {
            _jobs.CompleteAdding();
        }

        public bool IsCompleted()
        {
            return _jobs.IsCompleted;
        }

        public IJob Take()
        {
            return _jobs.Take();
        }

        public int Count()
        {
            return _jobs.Count;
        }

    }
}
