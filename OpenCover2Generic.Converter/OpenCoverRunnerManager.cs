﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using System.IO;
using BHGE.SonarQube.OpenCover2Generic.Factories;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner
{
    public class OpenCoverRunnerManager : IOpenCoverRunnerManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OpenCoverRunnerManager).Name);
        private string _testResultsPath;
        

        private readonly StringBuilder _processOutput = new StringBuilder(2048);
        private readonly IProcessFactory _processFactory;

        public OpenCoverRunnerManager(IProcessFactory processFactory)
        {
            _processFactory = processFactory;
        }

        public void Run(ProcessStartInfo startInfo, StreamWriter writer)
        {
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            writer.WriteLine("Arguments: " + startInfo.Arguments);

            int tries = 0;
            bool _registrationFailed = true;
            while (tries<10  && _registrationFailed)
            {
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
            writer.Write(_processOutput.ToString());
            if (tries==10)
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
    }
}