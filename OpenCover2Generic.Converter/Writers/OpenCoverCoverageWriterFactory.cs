using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Writers
{
    public class CoverageWriterFactory : ICoverageWriterFactory
    {
        public ICoverageWriter CreateOpenCoverCoverageWriter()
        {
            return new OpenCoverCoverageWriter();
        }
    }
}
