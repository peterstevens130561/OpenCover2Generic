using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public class MultiAssemblyConverter : IConverter
    {
        private readonly IModel _model;
        private readonly ICoverageWriter _coverageWriter;
        private readonly ICoverageParser _parser;
        private readonly ICoverageParser _moduleParser;
        private readonly ICoverageWriter _moduleWriter;
        public MultiAssemblyConverter(IModel model,ICoverageParser parser,ICoverageWriter coverageWriter)
        {
            _parser = parser;
            _model = model;
            _coverageWriter = coverageWriter;
            _moduleParser = new OpenCoverCoverageParser();
            _moduleWriter = new OpenCoverCoverageWriter();
        }

        public void Convert(StreamWriter writer, StreamReader reader)
        {
            using (XmlTextWriter xmlWriter = new XmlTextWriter(writer))
            {
                _coverageWriter.WriteBegin(xmlWriter);
                using (XmlReader xmlReader = XmlReader.Create(reader))
                {
                    xmlReader.MoveToContent();
                    while (_parser.ParseModule(_model,xmlReader))
                    {
                        string moduleFile = Path.GetTempFileName();
                        WriteModuleToFile(moduleFile);
                        _model.Clear();
                        ReadModuleFromFile(moduleFile);
                        _coverageWriter.GenerateCoverage(_model, xmlWriter);
                        _model.Clear();
                    };
                    _coverageWriter.GenerateCoverage(_model, xmlWriter);
                    _model.Clear();
                }
                _coverageWriter.WriteEnd(xmlWriter);
            }
        }

        private void ReadModuleFromFile(string moduleFile)
        {
            using (XmlReader tempFileReader = XmlReader.Create(moduleFile))
            {
                tempFileReader.MoveToContent();
                _moduleParser.ParseModule(_model, tempFileReader);
            }
        }

        private void WriteModuleToFile(string moduleFile)
        {
            using (XmlWriter tempFileWriter = new XmlTextWriter(moduleFile, Encoding.UTF8))
            {
                //write it to the temp file
                _moduleWriter.GenerateCoverage(_model, tempFileWriter);
            }

        }
    }
}
