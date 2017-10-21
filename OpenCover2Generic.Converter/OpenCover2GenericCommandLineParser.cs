using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public class OpenCover2GenericCommandLineParser : IOpenCover2GenericCommandLineParser
    {
        readonly ICommandLineParser _commandLineParser = new CommandLineParser();
        public OpenCover2GenericCommandLineParser(ICommandLineParser commandLineParser)
        {
            _commandLineParser = commandLineParser;
        }

        public string[] Args
        {
            get
            {
                return _commandLineParser.Args;
            }
            set
            {
                _commandLineParser.Args = value;
            }
        }

        public string GenericPath()
        {
            return _commandLineParser.GetArgument("-Generic:");
        }

        public string OpenCoverPath()
        {
            return _commandLineParser.GetArgument("-OpenCover:");
        }



    }
}
