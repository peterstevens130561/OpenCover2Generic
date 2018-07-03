using System;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module;
using BHGE.SonarQube.OpenCover2Generic.Parsers;

namespace BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage
{
    public class CoverageAggregate : ICoverageAggregate
    {
        private readonly IXmlAdapter _xmlAdapter;
        private readonly IOpenCoverageParserFactory _openCoverageParserFactory;


        public CoverageAggregate(string path,
            IOpenCoverageParserFactory openCoverageParserFactory, 
            IXmlAdapter xmlAdapter
            ) 
            
        {
            Path = path;
            _openCoverageParserFactory = openCoverageParserFactory;
            _xmlAdapter = xmlAdapter;
        }

        public string Path { get; private set; }

        public void Modules(Action<IModule> action)
        {
            var parser = _openCoverageParserFactory.Create();
            using (var xmlReader = _xmlAdapter.CreateReader(Path))
            {
                xmlReader.MoveToContent();
                var model = new Module();
                while (parser.ParseModule(model, xmlReader))
                {
                    action.Invoke(model);
                    model = new Module();
                }
            }

        }
    }
}
