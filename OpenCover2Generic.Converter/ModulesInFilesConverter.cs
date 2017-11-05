using BHGE.SonarQube.OpenCover2Generic;
using BHGE.SonarQube.OpenCover2Generic.Model;
using BHGE.SonarQube.OpenCover2Generic.Parsers;
using System.IO;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
{
    class ModulesInFilesConverter : IModulesInFilesConverter
    {
        private readonly IModuleCoverageModel _model;
        private readonly ICoverageWriter _coverageWriter;
        private readonly ICoverageParser _parser;

        public ModulesInFilesConverter(IModuleCoverageModel model, ICoverageParser parser, ICoverageWriter coverageWriter)
        {
            _parser = parser;
            _model = model;
            _coverageWriter = coverageWriter;
        }

        public void Convert(string root, string assembly, StreamReader reader)
        {
            using (XmlReader xmlReader = XmlReader.Create(reader))
            {
                xmlReader.MoveToContent();
                while (_parser.ParseModule(_model, xmlReader))
                {
                    WriteModule(root, assembly);
                    _model.Clear();
                };
                WriteModule(root, assembly);
            }
        }

        private void WriteModule(string root, string assembly)
        {
            string module = _parser.ModuleName;
            if (module == null)
            {
                return;
            }
            string outputPath = Path.Combine(root, module, assembly + ".xml");
            using (var fileWriter = new StreamWriter(outputPath))
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(fileWriter))
                {
                    _coverageWriter.WriteBegin(xmlWriter);
                    _coverageWriter.GenerateCoverage(_model, xmlWriter);
                    _coverageWriter.WriteEnd(xmlWriter);
                }
            }
        }
    }
}
