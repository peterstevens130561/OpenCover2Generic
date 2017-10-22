using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
{
    public class GenericCoverageWriter : ICoverageWriter
    {

        public void WriteBegin(XmlTextWriter xmlWriter)
        {
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.Indentation = 4;
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("coverage");
            xmlWriter.WriteAttributeString("version", "1");
        }
    }
}
