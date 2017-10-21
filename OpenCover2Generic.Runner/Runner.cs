﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner
{
    public class Runner
    {
        private string _path;
        private readonly StringBuilder _arguments = new StringBuilder(2048);
        private string _testResultsPath;

        public void Run()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(_path, _arguments.ToString());
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            using (var process = new Process())
            {
                process.StartInfo = startInfo;
                process.EnableRaisingEvents = true;
                process.OutputDataReceived += Process_OutputDataReceived;
                process.ErrorDataReceived += Process_OutputDataReceived;
                process.Exited += Process_Exited;
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                while (!process.HasExited)
                {
                    Thread.Sleep(10000);
                }
                process.OutputDataReceived -= Process_OutputDataReceived;
                process.ErrorDataReceived -= Process_OutputDataReceived;
                process.Exited -= Process_Exited;
                Console.WriteLine($"TestResults in {_testResultsPath}");

            }

        }

        public string TestResultsPath { get { return _testResultsPath; } }

        private void Process_Exited(object sender, EventArgs e)
        {
            _exited = true;
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if(e?.Data ==null)
            {
                return;
            }
            // silly defect in OpenCover, ignore
            if(e.Data.Contains("Could not find a part of the path"))
            {
                return;
            }
            if(e.Data.Contains("VsTestSonarQubeLogger.TestResults")) {
                string[] parts = e.Data.Split('=');
                if(parts.Length==2)
                {
                    _testResultsPath = parts[1];
                }
            }
            Console.WriteLine(e.Data);
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