using log4net;
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
        private int _executedTests = 0;
        private int _ignoredTests = 0;
        private static readonly ILog log = LogManager.GetLogger(typeof(TestResultsConcatenator));
        private readonly ICollection<string> paths = new Collection<string>();
        public XmlTextWriter Writer { get;set;}


        public int ExecutedTestCases
        {
            get
            {
                return _executedTests;
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
                    bool isEmpty = xmlReader.IsEmptyElement;
                    if(xmlReader.Name == "testCase") {
                        
                        if(doWrite)
                        {
                            ++_executedTests;
                        } else
                        {
                            ++_ignoredTests;
                        }
                    }
                    if (xmlReader.Name== "file")
                    {
                        string path = xmlReader.GetAttribute("path");
                        doWrite = IsFirstTimeSeen(path);
                        if(doWrite)
                        {
                            Writer.WriteStartElement(xmlReader.Name);
                            Writer.WriteAttributeString("path", path);
                            if (isEmpty)
                            {
                                Writer.WriteEndElement();
                            }

                        } else
                        {
                            log.Warn($"Skipping tests in {path}");
                        }
                    }
                    else if (doWrite)
                    {

                        CreateStartElement(xmlReader);
                    }

                }
                if (doWrite && xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name != "unitTest")
                {
                    Writer.WriteEndElement();
                }
            }
        }

        private bool IsFirstTimeSeen(string path)
        {
            bool seenFirstTime= !paths.Contains(path);
            if(seenFirstTime)
            {
                paths.Add(path);
            }
            return seenFirstTime;
        }
        private void CreateStartElement(XmlReader xmlReader)
        {
            bool isEmpty = xmlReader.IsEmptyElement; // has to be local, as value changes during scan of attributes
            if (isEmpty|| xmlReader.HasAttributes)
            {

                Writer.WriteStartElement(xmlReader.Name);
                if (xmlReader.HasAttributes)
                {
                    while (xmlReader.MoveToNextAttribute())
                    {
                        string value = xmlReader.Value;
                        string name = xmlReader.Name;
                        Writer.WriteAttributeString(name, value);
                    }
                }
                if (isEmpty)
                {
                    Writer.WriteEndElement();
                }
            }
        }

        public void Begin()
        {
            Writer.Formatting = Formatting.Indented;
            Writer.Indentation = 4;
            Writer.WriteStartDocument();
            Writer.WriteStartElement("unitTest");
            Writer.WriteAttributeString("version", "1");
        }

        public void End()
        {
            Writer.WriteEndElement();
            Writer.WriteEndDocument();
            Writer.Flush();
            log.Info($"Executed tests           : {_executedTests}");
            log.Info($"Duplicate (ignored) tests: {_ignoredTests}");
        }
    }
}
