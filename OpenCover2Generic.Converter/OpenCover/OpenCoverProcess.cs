using System;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.Utils;
using log4net;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCover
{
    internal class OpenCoverProcess : IOpenCoverProcess
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OpenCoverProcess).Name);
        private readonly IProcessAdapter _processAdapter;
        private static Object _lock = new Object();
        private readonly ITimerAdapter _watchDog;
        private readonly IStateMachine _stateMachine;
        public OpenCoverProcess(IProcessAdapter processAdapter,ITimerAdapter timer,IStateMachine stateMachine)
        { 
            _processAdapter = processAdapter;
            _watchDog = timer;
            _stateMachine = stateMachine;
            State = ProcessState.Starting;
        }

        public event DataReceivedEventHandler DataReceived { 
                        add
        {
            _processAdapter.DataReceived += value;
        }
        remove
            {
                _processAdapter.DataReceived -= value;
            }

}
        public bool HasExited
        {
            get
            {
                return _processAdapter.HasExited;
            }
        }


        public string TestResultsPath
        {
            get; private set;
        }

        public ProcessStartInfo StartInfo
        {
            get
            {
                return _processAdapter.StartInfo;
            }

            set
            {
                _processAdapter.StartInfo = value;
            }
        }

        public ProcessState State {
            get { return _stateMachine.State; }
            private set { _stateMachine.State = value; } 
        }
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
                log.Debug("Starting");
                DataReceived += Process_OutputDataReceived;
                _watchDog.Elapsed += OnTimeOut;
                _watchDog.Start();
                State = ProcessState.Starting;
                _processAdapter.Start();
                
                while (!_processAdapter.HasExited)
                {
                    Thread.Sleep(1000);
                }
                _watchDog.Elapsed -= OnTimeOut;

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
            _stateMachine.State = startState;
            _stateMachine.Transition(data);         
            return _stateMachine.State;
        }

        private void OnTimeOut(object sender, ElapsedEventArgs e)
        {
            State = ProcessState.TimedOut;
            _processAdapter.Kill();
        }

        public void Kill()
        {
            _processAdapter.Kill();
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _processAdapter.Dispose();
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