using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Writers;
using log4net;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Tests
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
                    doWrite = ParseElement(xmlReader, doWrite);

                }
                if (doWrite && xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name != "unitTest")
                {
                    Writer.WriteEndElement();
                }
            }
        }

        private bool ParseElement(XmlReader xmlReader, bool doWrite)
        {
            bool isEmpty = xmlReader.IsEmptyElement;
            if (xmlReader.Name == "testCase")
            {
                UpdateStatistics(doWrite);
            }
            if (xmlReader.Name == "file")
            {
                doWrite = ParseFile(xmlReader, isEmpty);
            }
            else if (doWrite)
            {
                CreateStartElement(xmlReader);
            }
            return doWrite;
        }

        private void UpdateStatistics(bool doWrite)
        {
            if (doWrite)
            {
                ++_executedTests;
            }
            else
            {
                ++_ignoredTests;
            }
        }

        private bool ParseFile(XmlReader xmlReader, bool isEmpty)
        {
            bool doWrite;
            string path = xmlReader.GetAttribute("path");
            doWrite = IsFirstTimeSeen(path);
            if (doWrite)
            {
                Writer.WriteStartElement(xmlReader.Name);
                Writer.WriteAttributeString("path", path);
                if (isEmpty)
                {
                    Writer.WriteEndElement();
                }

            }
            else
            {
                log.Warn($"Skipping tests in {path}");
            }

            return doWrite;
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
                CreateAttributes(xmlReader);
                if (isEmpty)
                {
                    Writer.WriteEndElement();
                }
            }
        }

        private void CreateAttributes(XmlReader xmlReader)
        {
            if (xmlReader.HasAttributes)
            {
                while (xmlReader.MoveToNextAttribute())
                {
                    string value = xmlReader.Value;
                    string name = xmlReader.Name;
                    Writer.WriteAttributeString(name, value);
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
