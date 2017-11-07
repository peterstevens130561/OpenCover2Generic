﻿
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Utils
{
    public class CommandLineParser : ICommandLineParser
    {
        public string[] Args { get; set; }
        public string GetArgument(string key)
        {
            if(Args==null)
            {
                throw new ArgumentNullException(nameof(key));
            }
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
            var realKey =  key+ ":";
            foreach (string arg in Args)
            {
                if (arg.ToUpper().StartsWith(realKey,StringComparison.CurrentCultureIgnoreCase))
                {
                    GetValue(arguments, realKey, arg);
                }
            }
            if (arguments.Count == 0) { 
            throw new ArgumentException($"commandline argument {key} missing");
        }
            return arguments.ToArray<string>();
        }

        private static void GetValue(Collection<string> arguments, string realKey, string arg)
        {
            string value = arg.Substring(realKey.Length);
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
}