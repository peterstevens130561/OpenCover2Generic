using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public class Jobs : IJobs, IDisposable
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _jobs.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
