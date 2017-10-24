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

        public MultiAssemblyConverter(IModel model,ICoverageParser parser,ICoverageWriter coverageWriter)
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
                    };
                    _coverageWriter.GenerateCoverage(_model, xmlWriter);
                    _model.Clear();
                }
                _coverageWriter.WriteEnd(xmlWriter);
            }
        }
    }
}
