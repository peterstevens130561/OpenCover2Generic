using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using System.IO;
using BHGE.SonarQube.OpenCover2Generic.Factories;
using BHGE.SonarQube.OpenCover2Generic.Utils;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner
{
    public class OpenCoverRunnerManager : IOpenCoverRunnerManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OpenCoverRunnerManager).Name);
        private string _path;
        private readonly StringBuilder _arguments = new StringBuilder(2048);
        private string _testResultsPath;
        private StreamWriter _writer;
        private bool _registrationFailed;
        private bool _started;

        private readonly IProcessFactory _processFactory;

        public OpenCoverRunnerManager(IProcessFactory processFactory)
        {
            _processFactory = processFactory;
        }
        public void Run(StreamWriter writer)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(_path, _arguments.ToString());
            Run(startInfo, writer);
        }

        public void Run(ProcessStartInfo startInfo, StreamWriter writer)
        {
            _writer = writer;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            _writer.WriteLine("Arguments: " + startInfo.Arguments);

            int tries = 0;
            _registrationFailed = true;
            while (tries<10  && _registrationFailed)
            {
                _registrationFailed = false;
                _started = false;
                using (IOpenCoverProcess process = _processFactory.CreateOpenCoverProcess())
                {
                    process.DataReceived += Process_OutputDataReceived;
                    process.StartInfo = startInfo;
                    process.Start();
                    while (!process.HasExited)
                    {
                        Thread.Sleep(1000);
                    }
                    process.DataReceived -= Process_OutputDataReceived;
                    _registrationFailed = process.RecoverableError;
                    if (_registrationFailed)
                    {
                        ++tries;
                    }
                    _testResultsPath = process.TestResultsPath;
                }

            }
            if(tries==10)
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
            _writer.WriteLine(e.Data);
        }
    }
}