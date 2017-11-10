using System;
using System.Diagnostics;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using System.Threading;
using System.IO;
using log4net;

namespace BHGE.SonarQube.OpenCover2Generic.Factories
{
    internal class OpenCoverProcess : IOpenCoverProcess
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OpenCoverProcess).Name);
        private IProcess _process;

        public OpenCoverProcess(IProcess process)
        {
            _process = process;
        }

        public event DataReceivedEventHandler DataReceived { 
                        add
        {
            _process.DataReceived += value;
        }
        remove
            {
                _process.DataReceived -= value;
            }

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
            get; private set;
        }

        public bool Started
        {
            get; private set;
        }

        public string TestResultsPath
        {
            get; private set;
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


        public void Start()
        {
            _process.DataReceived += Process_OutputDataReceived;
            Started = false;
            RecoverableError = false;
            _process.Start();

        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            // yes, this does happen
            if (e?.Data == null)
            {
                return;
            }

            // there is no other way to find out where vstest stores his
            //testresults
            if (e.Data.Contains("VsTestSonarQubeLogger.TestResults"))
            {
                string[] parts = e.Data.Split('=');
                if (parts.Length == 2)
                {
                    TestResultsPath = parts[1];
                }
            }

            if (e.Data.Contains("Starting test execution, please wait.."))
            {
                log.Debug("Started");
                Started = true;
            }

            if (e.Data.Contains("Failed to register(user:True"))
            {
                log.Error("Failed to start, could not register");
                RecoverableError = true;
            }
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