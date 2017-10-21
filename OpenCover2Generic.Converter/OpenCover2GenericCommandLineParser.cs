using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public class OpenCover2GenericCommandLineParser : IOpenCover2GenericCommandLineParser
    {
        readonly ICommandLineParser commandLineParser = new CommandLineParser();
        public string[] Args
        {
            get
            {
                return commandLineParser.Args;
            }
            set
            {
                commandLineParser.Args = value;
            }
        }

        public string GenericPath()
        {
            return commandLineParser.GetArgument("-Generic:");
        }

        public string OpenCoverPath()
        {
            return commandLineParser.GetArgument("-OpenCover:");
        }



    }
}
