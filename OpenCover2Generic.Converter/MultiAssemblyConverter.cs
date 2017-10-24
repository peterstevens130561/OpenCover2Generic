using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public class MultiAssemblyConverter 
    {
        private readonly IModel _model;
        private readonly ICoverageWriter _coverageWriter;
        private readonly ICoverageParser _parser;
        private readonly ICoverageParser _moduleParser;
        private readonly ICoverageWriter _moduleWriter;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="testCoverageParser"></param>
        /// <param name="resultCoverageWriter"></param>
        public MultiAssemblyConverter(IModel model,
            ICoverageParser testCoverageParser,
            ICoverageWriter resultCoverageWriter,
            ICoverageParser intermediateCoverageParser,
            ICoverageWriter intermediateCoverageWriter)
        {
            _parser = testCoverageParser;
            _model = model;
            _coverageWriter = resultCoverageWriter;
            _moduleParser = intermediateCoverageParser;
            _moduleWriter = intermediateCoverageWriter;
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
                        if (_model.GetCoverage().Count == 0)
                        {
                            continue;
                        }
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


        private void WriteModuleToFile(string moduleFile)
        {
            using (XmlWriter tempFileWriter = new XmlTextWriter(moduleFile, Encoding.UTF8))
            {
                //write it to the temp file
                _moduleWriter.GenerateCoverage(_model, tempFileWriter);
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

    }
}
