using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OpenCover2Generic.Converter
{
    public class FilteringTestResultsConcatenator : ITestResultsConcatenator
    {
        private int _tests = 0;
        private XmlTextWriter _xmlWriter;
        private string _file;
        private string _name;
        private string _duration;
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

        public int TestCases
        {
            get
            {
                return _tests;
            }
        }

        public void Concatenate(XmlReader xmlReader)
        {
            xmlReader.MoveToContent();
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    switch (xmlReader.Name)
                    {
                        case "file":
                            AddFile(xmlReader);
                            break;
                        case "testCase":
                            AddTestCase(xmlReader);
                            break;
                        case "error":
                            AddError(xmlReader);
                            break;
                        case "failure":
                            AddFailure(xmlReader);
                            break;
                        case "skipped":
                            AddSkipped(xmlReader);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void AddSkipped(XmlReader xmlReader)
        {
            throw new NotImplementedException();
        }

        private void AddFailure(XmlReader xmlReader)
        {
            throw new NotImplementedException();
        }

        private void AddError(XmlReader xmlReader)
        {
            
        }

        private void AddTestCase(XmlReader xmlReader)
        {
            ++_tests;
            _name = xmlReader.GetAttribute("name");
            string duration = xmlReader.GetAttribute("duration");

        }

        private void AddFile(XmlReader xmlReader)
        {
            _file = xmlReader.GetAttribute("path");
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
