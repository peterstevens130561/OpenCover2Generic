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
using BHGE.SonarQube.OpenCover2Generic.Seams;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner
{
    public class OpenCoverRunnerManager : IOpenCoverRunnerManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OpenCoverRunnerManager).Name);
        private string _testResultsPath;
        private readonly ITimerSeam _watchDog ;
        private readonly StringBuilder _processOutput = new StringBuilder(2048);
        private readonly IProcessFactory _processFactory;
        private bool _timeOut;
        private readonly Stopwatch _stopWatch = new Stopwatch();
        enum ProcessState
        {
            None,
            Busy,
            RecoverableFailure,
            TimedOut,
            Done
        }

        private ProcessState _processState;

        public OpenCoverRunnerManager(IProcessFactory processFactory, ITimerSeam timer)
        {
            _processFactory = processFactory;
            _watchDog = timer;
        }

        public void SetTimeOut(TimeSpan timeOut)
        {
            if (timeOut.TotalMilliseconds > 0)
            {
                _watchDog.Interval = timeOut.TotalMilliseconds;
                _watchDog.AutoReset = false;
                _watchDog.Elapsed += OnTimeOut;
            }
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
                    _stopWatch.Start();
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
                        _processState = tries < 10 ? ProcessState.Busy : ProcessState.RecoverableFailure;
                    }
                    else {
                        _processState = ProcessState.Done;
                        _testResultsPath = process.TestResultsPath;
                    }
                }
            }
            _watchDog.Stop();
            log.Debug($"Completed after {_stopWatch.Elapsed.TotalSeconds}s");
            writer.Write(_processOutput.ToString());
            if (_processState==ProcessState.RecoverableFailure)
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
                    log.Warn($"Did not find line 'VsTestSonarQubeLogger.TestResults=' in log: \n{_processOutput.ToString()}");

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
            log.Error($"Timeout occurred {_stopWatch.ElapsedMilliseconds}");
            _timeOut = true;
        }
    }
}