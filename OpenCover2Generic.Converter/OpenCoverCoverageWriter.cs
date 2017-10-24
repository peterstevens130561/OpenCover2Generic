

using System;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public class OpenCoverCoverageWriter : ICoverageWriter
    {
        public void GenerateCoverage(IModel model, XmlWriter xmlWriter)
        {
            if(model.GetCoverage().Count ==0)
            {
                return;
            }
            xmlWriter.WriteStartElement("Modules");
            xmlWriter.WriteStartElement("Module");
            WriteFilesElement(model, xmlWriter);

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
        }

        private static void WriteFilesElement(IModel model, XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("Files");
            foreach (ISourceFileCoverageModel fileCoverage in model.GetCoverage())
            {
                //<File uid="1" fullPath="E:\Cadence\EsieTooLinkRepositoryServiceTest.cs" />
                xmlWriter.WriteStartElement("File");
                //xmlWriter.WriteAttributeString("uid", fileCoverage.Id);
                xmlWriter.WriteAttributeString("fullPath", fileCoverage.FullPath);
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
        }

        public void WriteBegin(XmlTextWriter xmlWriter)
        {
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.Indentation = 4;
            xmlWriter.WriteStartDocument();
            //xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"
            xmlWriter.WriteStartElement("CoverageSession");
            xmlWriter.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
            xmlWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
        }

        public void WriteEnd(XmlWriter xmlWriter)
        {
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
        }
    }
}
