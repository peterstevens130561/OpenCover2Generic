using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OpenCover2Generic.Converter
{
    public class TestResultsConcatenator : ITestResultsConcatenator
    {
        private int _tests = 0;
        private XmlTextWriter _xmlWriter;
        private ICollection<string> paths = new Collection<string>();
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
            bool doWrite = true;
            xmlReader.MoveToContent();
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if(xmlReader.Name== "file")
                    {
                        bool isEmpty = xmlReader.IsEmptyElement;
                        string path = xmlReader.GetAttribute("path");
                        doWrite = !paths.Contains(path);
                        if(doWrite)
                        {
                            paths.Add(path);
                            _xmlWriter.WriteStartElement(xmlReader.Name);
                            _xmlWriter.WriteAttributeString("path", path);
                            if (isEmpty)
                            {
                                _xmlWriter.WriteEndElement();
                            }

                        }
                    }
                    else if (doWrite)
                    {
                        if (xmlReader.Name == "testCase")
                        {
                            ++_tests;
                        }
                        CreateStartElement(xmlReader);
                    }

                }
                if (doWrite && xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name != "unitTest")
                {
                    _xmlWriter.WriteEndElement();
                }
            }
        }

        private void CreateStartElement(XmlReader xmlReader)
        {
            bool isEmpty = xmlReader.IsEmptyElement; // has to be local, as value changes during scan of attributes
            if (isEmpty|| xmlReader.HasAttributes)
            {

                _xmlWriter.WriteStartElement(xmlReader.Name);
                if (xmlReader.HasAttributes)
                {
                    while (xmlReader.MoveToNextAttribute())
                    {
                        string value = xmlReader.Value;
                        string name = xmlReader.Name;
                        _xmlWriter.WriteAttributeString(name, value);
                    }
                }
                if (isEmpty)
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
