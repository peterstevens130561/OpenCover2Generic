using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.OpenCover;

namespace BHGE.SonarQube.OpenCover2Generic.Parsers
{

    public class OpenCoverageParserFactory : IOpenCoverageParserFactory
    {

        public ICoverageParser Create()
        {
            return new OpenCoverCoverageParser();
        }
    }
}
