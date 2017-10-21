using BHGE.SonarQube.OpenCover2Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCoverWrapper
{
    class OpenCoverWrapperCommandLineParser : IOpenCoverWrapperCommandLineParser
    {
        private readonly ICommandLineParser _commandLineParser;

        public OpenCoverWrapperCommandLineParser(ICommandLineParser commandLineParser)
        {
            _commandLineParser = commandLineParser;
        }

        public string[] Args
        {
            get { return _commandLineParser.Args; }
            set { _commandLineParser.Args = value; }
        }

        public string GetOutputPath()
        {
            return _commandLineParser.GetArgument("-output");
        }

        public string GetTargetArgs()
        {
            return _commandLineParser.GetArgument("-targetargs");
        }

        public string GetTargetPath()
        {
            return _commandLineParser.GetArgument("-target");
        }
    }
}
