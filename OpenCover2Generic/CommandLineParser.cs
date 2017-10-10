using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic
{
    class CommandLineParser : ICommandLineParser
    {
        public string[] Args { get; set; }

        public string GenericPath()
        {
            return GetArgument("-Generic:");
        }

        public string OpenCoverPath()
        {
            return GetArgument("-OpenCover:");
        }


        private string GetArgument(string key)
        {
            foreach(string arg in Args) {
                if(arg.StartsWith(key))
                {
                    return (arg.Substring(key.Length));
                }
            }
            throw new ArgumentException($"commandline argument {key} missing");
        }
    }
}
