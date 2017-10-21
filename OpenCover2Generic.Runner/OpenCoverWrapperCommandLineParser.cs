using BHGE.SonarQube.OpenCover2Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner
{
    class OpenCoverWrapperCommandLineParser : IOpenCoverCommandLineParser
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
    }
}
