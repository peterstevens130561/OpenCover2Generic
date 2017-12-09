using System;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using BHGE.SonarQube.OpenCover2Generic.Factories;
using BHGE.SonarQube.OpenCover2Generic.Seams;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using log4net;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCover
{
    internal class OpenCoverProcess : IOpenCoverProcess
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OpenCoverProcess).Name);
        private readonly IProcess _process;
        private static Object _lock = new Object();
        private readonly ITimerSeam _watchDog;

        public OpenCoverProcess(IProcess process,ITimerSeam timer)
        {
            _process = process;
            _watchDog = timer;
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
        public void SetTimeOut(TimeSpan timeOut)
        {
            if (timeOut.TotalMilliseconds > 0)
            {
                _watchDog.Interval = timeOut.TotalMilliseconds;
                _watchDog.AutoReset = false;
                
            }
        }

        public void Start()
        {
            lock( _lock) {
                log.Debug("Starting");
                DataReceived += Process_OutputDataReceived;
                Started = false;
                _watchDog.Elapsed += OnTimeOut;
                _watchDog.Start();
                State = ProcessState.Busy;
                _process.Start();
                
                while (!Started && !_process.HasExited)
                {
                    Thread.Sleep(1000);
                }
                _watchDog.Elapsed -= OnTimeOut;
            }

        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            // yes, this does happen
            if (e?.Data == null)
            {
                return;
            }
            string data = e.Data;
            // there is no other way to find out where vstest stores his
            //testresults
            if (data.Contains("VsTestSonarQubeLogger.TestResults"))
            {
                string[] parts = e.Data.Split('=');
                if (parts.Length == 2)
                {
                    TestResultsPath = parts[1];
                }
            }

            State=OpenCoverOutputStateMachine(data,State);

        }

        private ProcessState OpenCoverOutputStateMachine(string data, ProcessState startState)
        {
            ProcessState resultState = startState;
            if (startState == ProcessState.Busy)
            {
                if (data.Contains("Starting test execution, please wait.."))
                {
                    log.Debug("Started");
                    Started = true;
                    resultState = ProcessState.Busy;
                }

                if (data.Contains("Failed to register(user:True"))
                {
                    log.Error("Failed to start, could not register");
                    resultState = ProcessState.CouldNotRegister;
                }
                if (data.StartsWith("No test is available in"))
                {
                    resultState = ProcessState.NoTests;
                }
                if (data.Contains("No results, this could be for a number of reasons"))
                {
                    log.Error("No results");
                    resultState = ProcessState.NoResults;
                }

                if (data.Contains("Visited Classes"))
                {
                    resultState = ProcessState.Done;
                }
            }

            return resultState;
        }

        private void OnTimeOut(object sender, ElapsedEventArgs e)
        {
            State = ProcessState.TimedOut;
            _process.Kill();
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