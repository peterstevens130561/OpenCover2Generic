using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Exceptions;
using log4net;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCover
{
    public class OpenCoverRunnerManager : IOpenCoverRunnerManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(OpenCoverRunnerManager).Name);
        private string _testResultsPath;
        private readonly StringBuilder _processOutput = new StringBuilder(2048);
        private readonly IOpenCoverProcessFactory _processFactory;
        private readonly Stopwatch _stopWatch = new Stopwatch();
        private static object _lock = new object();
        private static object _queueLock = new object();
        private static int queued = 0;

        private ProcessState _processState;
        private TimeSpan _timeOut;

        public OpenCoverRunnerManager(IOpenCoverProcessFactory processFactory)
        {
            _processFactory = processFactory;
            WaitTimeSpan = new TimeSpan(0, 0, 1);
        }

        public void SetTimeOut(TimeSpan timeOut)
        {
            _timeOut = timeOut;
        }

        public TimeSpan WaitTimeSpan { get; set; }

        public void Run(ProcessStartInfo startInfo, StreamWriter writer,string jobAssemblies)
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
                using (IOpenCoverProcess process = _processFactory.Create())
                {
                    process.DataReceived += Process_OutputDataReceived;
                    process.StartInfo = startInfo;
                    process.SetTimeOut(_timeOut);
                    _log.Debug("Waiting");
                    changeQueue(1);
                    lock (_lock)
                    {
                        changeQueue(-1);
                        _log.Debug("Starting");
                        Task task = Task.Run(() => process.Start());
                        while (!task.IsCompleted && process.State == ProcessState.Starting)
                        {
                            Thread.Sleep((int)WaitTimeSpan.TotalMilliseconds);
                        }
                    }
                    _log.Debug("Running");
                    _stopWatch.Start();

                    while (!process.HasExited)
                    {
                        Thread.Sleep((int)WaitTimeSpan.TotalMilliseconds);
                    }
                    writer.Write(_processOutput.ToString());
                    writer.Write(process.State);
                    _processOutput.Clear();
                    process.DataReceived -= Process_OutputDataReceived;
                    _log.Debug($"Process ended with state {process.State}");
                    switch (process.State)
                    {
                        case ProcessState.TimedOut:
                            string msg = $"Test times out: {jobAssemblies}";
                            _log.Error(msg);
                            throw new JobTimeOutException(msg);
                        case ProcessState.NoTests:
                            msg = $"No tests in : {jobAssemblies}";
                            _log.Warn(msg);
                            _processState = ProcessState.NoTests;
                            break;
                        case ProcessState.Done:
                            _processState = ProcessState.Done;
                            _testResultsPath = process.TestResultsPath;
                            msg = $"Completed     : {jobAssemblies}";
                            _log.Info(msg);
                            break;
                        case ProcessState.NoResults:
                            _processState = ProcessState.NoResults;
                            msg = $"No result {tries}    : {jobAssemblies}";
                            _log.Error(msg);
                            ++tries;
                            _processState = tries < 10 ? ProcessState.Busy : ProcessState.NoResults;
                            break;
                        case ProcessState.Starting:
                            _log.Warn($"Could not start             : {jobAssemblies}");
                            tries = RetryPossible(writer, tries);
                            break;
                        case ProcessState.CouldNotRegister:
                            _log.Warn($"Could not register           : {jobAssemblies}");
                            tries = RetryPossible(writer, tries);
                            break;
                        case ProcessState.RunningTests:
                            _log.Warn($"Crashed during running tests : {jobAssemblies}");
                            tries = RetryPossible(writer, tries);
                            break;
                        default:
                            throw new InvalidOperationException($"Unexpected process state {process.State}");
                    }

                }
            }
            _log.Debug($"Completed after {_stopWatch.Elapsed.TotalSeconds}s");
            writer.Write(_processOutput.ToString());
            if (_processState==ProcessState.RecoverableFailure)
            {
                string msg = $"Could not start OpenCover, due to registration problem: {jobAssemblies}";
                _log.Error(msg);
                throw new InvalidOperationException(msg);
            }
        }

        private void changeQueue(int change)
        {
            lock (_queueLock)
            {
                queued += change;
                _log.Debug($"Queued {queued}");
            }
        }
        private int RetryPossible(StreamWriter writer, int tries)
        {
            ++tries;
            _processState = tries < 10 ? ProcessState.Busy : ProcessState.RecoverableFailure;
            writer.WriteLine("----Retrying---");
            return tries;
        }

        public string TestResultsPath
        {
            get
            {
                if (_testResultsPath == null)
                {
                    _log.Warn($"Did not find line 'VsTestSonarQubeLogger.TestResults=' in _log: \n{_processOutput.ToString()}");

                }
                return _testResultsPath;
            }
        }

        public bool HasTests
        {
            get { return _processState != ProcessState.NoTests; }

            set
            {
                throw new NotImplementedException();
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
    }
}