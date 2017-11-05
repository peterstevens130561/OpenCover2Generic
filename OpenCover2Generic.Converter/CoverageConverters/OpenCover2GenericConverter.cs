using BHGE.SonarQube.OpenCover2Generic.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Parsers;
using BHGE.SonarQube.OpenCover2Generic.Writers;

namespace BHGE.SonarQube.OpenCover2Generic.CoverageConverters
{
    public class OpenCover2GenericConverter : ICoverageConverter
    {
        private readonly IModuleCoverageModel _model;
        private readonly ICoverageWriter _coverageWriter;
        private readonly ICoverageParser _parser;

        public OpenCover2GenericConverter(IModuleCoverageModel model,ICoverageParser parser,ICoverageWriter coverageWriter)
        {
            _parser = parser;
            _model = model;
            _coverageWriter = coverageWriter;
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
                        _coverageWriter.GenerateCoverage(_model, xmlWriter);
                        _model.Clear();
                    }
                    _coverageWriter.GenerateCoverage(_model, xmlWriter);
                    _model.Clear();
                }
                _coverageWriter.WriteEnd(xmlWriter);
            }
        }
    }
}
