using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.Parsers;

namespace BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage
{
    public class CoverageAggregateFactory : ICoverageAggregateFactory
    {
        private readonly IOpenCoverageParserFactory _coverageParserFactory;

        public CoverageAggregateFactory() : this(new OpenCoverageParserFactory())
        {
            
        }
        public CoverageAggregateFactory(IOpenCoverageParserFactory coverageParserFactory)
        {
            _coverageParserFactory = coverageParserFactory;
        }

        public ICoverageAggregate Create(string path)
        {
            return new CoverageAggregate(path, _coverageParserFactory,new XmlAdapter());
        }
    }
}
