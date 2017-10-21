using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner
{
    public class Runner
    {
        private string _path;
        private readonly StringBuilder _arguments = new StringBuilder(2048);
        public void Run()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(_path, _arguments.ToString());
            startInfo.CreateNoWindow = true;
            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

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