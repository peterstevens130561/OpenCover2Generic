using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Parsers;

namespace BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage
{
    public class CoverageAggregateFactory : ICoverageAggregateFactory
    {
        private readonly IOpenCoverageParserFactory _coverageParserFactory;

        public CoverageAggregateFactory(IOpenCoverageParserFactory coverageParserFactory)
        {
            _coverageParserFactory = coverageParserFactory;
        }

        public ICoverageAggregate Create(string path, string key)
        {
            return new CoverageAggregate(path, key, _coverageParserFactory);
        }
    }
}
