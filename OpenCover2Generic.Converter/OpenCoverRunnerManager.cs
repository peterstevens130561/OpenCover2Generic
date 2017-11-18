using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.IO;
using BHGE.SonarQube.OpenCover2Generic.Factories;
using System.Timers;
using System.Threading;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner
{
    public class OpenCoverRunnerManager : IOpenCoverRunnerManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OpenCoverRunnerManager).Name);
        private string _testResultsPath;
        private readonly System.Timers.Timer _watchDog ;

        private readonly StringBuilder _processOutput = new StringBuilder(2048);
        private readonly IProcessFactory _processFactory;
        private bool _timeOut;

        enum ProcessState
        {
            None,
            Busy,
            RegistrationFailure,
            TimedOut,
            Done
        }

        private ProcessState _processState;

        public OpenCoverRunnerManager(IProcessFactory processFactory, System.Timers.Timer timer)
        {
            _processFactory = processFactory;
            _watchDog = timer;
        }

        public void SetTimeOut(TimeSpan timeOut)
        {
            _watchDog.Interval = timeOut.Milliseconds;
            _watchDog.AutoReset = false;
            _watchDog.Elapsed += OnTimeOut;

        }




        public bool TimedOut { get; private set; }

        public void Run(ProcessStartInfo startInfo, StreamWriter writer)
        {
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            writer.WriteLine("Arguments: " + startInfo.Arguments);

            int tries = 0;
            _processState = ProcessState.Busy;
            while (_processState== ProcessState.Busy)
            {
                using (IOpenCoverProcess process = _processFactory.CreateOpenCoverProcess())
                {
                    process.DataReceived += Process_OutputDataReceived;
                    process.StartInfo = startInfo;

                    process.Start();
                    _watchDog.Start();
                    while (!process.HasExited && ! _timeOut)
                    {
                        Thread.Sleep(1000);
                    }
                    process.DataReceived -= Process_OutputDataReceived;
                    if (_timeOut)
                    {
                        process.Kill();
                        TimedOut = true;
                        _processState = ProcessState.TimedOut;
                    }
                    else if (process.RecoverableError)
                    {
                        ++tries;
                        _processState = tries < 10 ? ProcessState.Busy : ProcessState.RegistrationFailure;
                    }
                    else {
                        _processState = ProcessState.Done;
                        _testResultsPath = process.TestResultsPath;
                    }
                }
            }
            writer.Write(_processOutput.ToString());
            if (_processState==ProcessState.RegistrationFailure)
            {
                throw new InvalidOperationException("Could not start OpenCover, due to registration problems");
            }

        }

        public string TestResultsPath
        {
            get
            {
                if (_testResultsPath == null)
                {
                    log.Warn("Did not find line 'VsTestSonarQubeLogger.TestResults=' in log ");
                }
                return _testResultsPath;
            }
        }


        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            // yes, this does happen
            if(e?.Data ==null)
            {
                return;
            }
            _processOutput.AppendLine(e.Data);

        }

        private void OnTimeOut(object sender, ElapsedEventArgs e)
        {
            log.Error("Timeout occurred");
            _timeOut = true;
        }
    }
}