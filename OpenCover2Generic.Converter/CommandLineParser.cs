
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public class CommandLineParser : ICommandLineParser
    {
        public string[] Args { get; set; }
        public string GetArgument(string key)
        {
            key = key.ToUpper() + ":" ;
            foreach (string arg in Args)
            {
                if (arg.ToUpper().StartsWith(key))
                {
                    return (arg.Substring(key.Length));
                }
            }
            throw new ArgumentException($"commandline argument {key} missing");
        }
    }
}
