using BHGE.SonarQube.OpenCover2Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner
{
    class OpenCoverWrapperCommandLineParser
    {
        private readonly ICommandLineParser _commandLineParser;

        public OpenCoverWrapperCommandLineParser(ICommandLineParser commandLineParser)
        {
            _commandLineParser = commandLineParser;
        }
    }
}
