using System;
using System.Diagnostics;
using BHGE.SonarQube.OpenCover2Generic.Utils;

namespace BHGE.SonarQube.OpenCover2Generic.Factories
{
    internal class OpenCoverProcess : IOpenCoverProcess
    {
        private IProcess _process;

        public OpenCoverProcess(IProcess process)
        {
            _process = process;
        }

        public bool HasExited
        {
            get
            {
                return _process.HasExited;
            }
        }

        public bool RecoverableError
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Started
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ProcessStartInfo StartInfo
        {
            get
            {
                return _process.StartInfo;
            }

            set
            {
                _process.StartInfo = value;
            }
        }

        public event DataReceivedEventHandler DataReceived;

        public void Start()
        {
            _process.Start();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _process.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~OpenCoverProcess() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}