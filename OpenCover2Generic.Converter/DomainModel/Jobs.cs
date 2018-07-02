using System;
using System.Collections.Concurrent;

namespace BHGE.SonarQube.OpenCover2Generic.DomainModel
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
        private bool _disposed; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _jobs.Dispose();
                }

                _disposed = true;
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
