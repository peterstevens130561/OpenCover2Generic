using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OpenCover2Generic.Converter
{
    class TestResultsConcatenator : ITestResultsConcatenator
    {
        private XmlTextWriter _xmlWriter;

        public XmlTextWriter Writer
        {
            get
            {
                return _xmlWriter;
            }

            set
            {
                _xmlWriter = value;
            }
        }

        public void Concatenate(XmlReader xmlReader)
        {
            xmlReader.MoveToContent();
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    _xmlWriter.WriteStartElement(xmlReader.Name);
                    if(xmlReader.HasAttributes)
                    {
                        while(xmlReader.MoveToNextAttribute())
                        {
                            string value = xmlReader.Value;
                            string name = xmlReader.Name;
                            _xmlWriter.WriteAttributeString(name, value);
                        }
                    }
                }
                if(xmlReader.NodeType == XmlNodeType.EndElement)
                {
                    _xmlWriter.WriteEndElement();
                }
            }
        }

        public void Begin()
        {
            _xmlWriter.Formatting = Formatting.Indented;
            _xmlWriter.Indentation = 4;
            _xmlWriter.WriteStartDocument();
            _xmlWriter.WriteStartElement("unitTest");
            _xmlWriter.WriteAttributeString("version", "1");
        }

        public void End()
        {
            _xmlWriter.WriteEndElement();
            _xmlWriter.WriteEndDocument();
            _xmlWriter.Flush();
        }
    }
}
