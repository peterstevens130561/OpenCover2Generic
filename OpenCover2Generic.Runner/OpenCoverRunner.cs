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
    public class OpenCoverRunnerManager
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
                using (IProcess process = _processFactory.CreateProcess())
                {
                    process.DataReceived += Process_OutputDataReceived;
                    process.StartInfo = startInfo;
                    process.Start();
                    while (!process.HasExited)
                    {
                        Thread.Sleep(1000);
                    }
                    process.DataReceived -= Process_OutputDataReceived;

                }
                if (_registrationFailed)
                {
                    ++tries;
                }
            }
            if(_registrationFailed)
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

            // there is no other way to find out where vstest stores his
            //testresults
            if(e.Data.Contains("VsTestSonarQubeLogger.TestResults")) {
                string[] parts = e.Data.Split('=');
                if(parts.Length==2)
                {
                    _testResultsPath = parts[1];
                }
            }

            if(e.Data.Contains("Starting test execution, please wait.."))
            {
                log.Debug("Started");
            }
            _writer.WriteLine(e.Data);

            if (e.Data.Contains("Failed to register(user:True"))
            {
                log.Error("Failed to start, could not register");
                _registrationFailed = true;
            }
        }

        public void AddArgument(string argument)
        {
            _arguments.Append(argument).Append(" ");
        }

        public void SetPath(string path)
        {
            _path = path;
        }


    }
}