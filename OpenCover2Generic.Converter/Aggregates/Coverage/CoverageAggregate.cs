using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.Model;
using BHGE.SonarQube.OpenCover2Generic.Parsers;
using BHGE.SonarQube.OpenCover2Generic.Writers;

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

        public string Key { get; private set; }
        public string Path { get; private set; }

        public void Modules(Action<IModuleCoverageModel> action)
        {
            var parser = _openCoverageParserFactory.Create();
            try
            {
                using (var xmlReader = _xmlAdapter.CreateReader(Path))
                {
                    xmlReader.MoveToContent();
                    var model = new ModuleCoverageModel();
                    while (parser.ParseModule(model, xmlReader))
                    {
                        action.Invoke(model);
                        model = new ModuleCoverageModel();
                    }
                }

            }
            catch (Exception e)
            {
                //_log.Error($"Exception thrown during reading {path}\n{e.Message}\n{e.StackTrace}");
                throw;
            }
        }
    }
}
