
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public string[] GetArgumentArray(string key)
        {
            var arguments = new Collection<String>();
            key = key.ToUpper() + ":";
            foreach (string arg in Args)
            {
                if (arg.ToUpper().StartsWith(key))
                {

                    string value = arg.Substring(key.Length);
                    if (value.Contains(","))
                    {
                        foreach (var part in value.Split(','))
                        {
                            arguments.Add(part);
                        }
                    }
                    else
                    {
                        arguments.Add(value);
                    }
                }
            }
            if (arguments.Count == 0) { 
            throw new ArgumentException($"commandline argument {key} missing");
        }
            return arguments.ToArray<string>();
        }
    }
}
