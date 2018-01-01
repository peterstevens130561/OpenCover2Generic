using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Model;
using BHGE.SonarQube.OpenCover2Generic.Parsers;

namespace BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage
{
    public class CoverageAggregate : ICoverageAggregate
    {
        private readonly IOpenCoverageParserFactory _openCoverageParserFactory;

        public CoverageAggregate(string path, string key,IOpenCoverageParserFactory openCoverageParserFactory)
        {
            Path = path;
            Key = key;
            _openCoverageParserFactory = openCoverageParserFactory;
        }

        public string Key { get; private set; }
        public string Path { get; private set; }

        public void Modules(Action<IntermediateModel> action)
        {
            action.Invoke(null);
        }
    }
}
