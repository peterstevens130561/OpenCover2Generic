using System;
using System.Diagnostics;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using System.Threading;
using System.IO;
using log4net;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;

namespace BHGE.SonarQube.OpenCover2Generic.Factories
{
    internal class OpenCoverProcess : IOpenCoverProcess
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OpenCoverProcess).Name);
        private readonly IProcess _process;
        private static Object _lock = new Object();

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

        public ProcessState State { get; private set; }

        public void Start()
        {
            lock( _lock) {
                log.Debug("Starting");
                DataReceived += Process_OutputDataReceived;
                Started = false;
                _process.Start();
                while(!Started && !_process.HasExited)
                {
                    Thread.Sleep(1000);
                }
            }

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
                State = ProcessState.Busy;
            }

            if (e.Data.Contains("Failed to register(user:True"))
            {
                log.Error("Failed to start, could not register");
                State = ProcessState.CouldNotRegister;
            }

            if (e.Data.Contains("No results, this could be for a number of reasons"))
            {
                log.Error("No results");
                State = ProcessState.NoResults;
            }
        }

        public void Kill()
        {
            _process.Kill();
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _process.Dispose();
                }
                _disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}