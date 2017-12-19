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
        private readonly BlockingCollection<ITestJob> _jobs = new BlockingCollection<ITestJob>();

        public void Add(ITestJob testJob)
        {
            _jobs.Add(testJob);
        }

        public void CompleteAdding()
        {
            _jobs.CompleteAdding();
        }

        public bool IsCompleted()
        {
            return _jobs.IsCompleted;
        }

        public ITestJob Take()
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
