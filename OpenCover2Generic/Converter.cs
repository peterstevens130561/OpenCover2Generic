﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic
{
    internal class Converter : IConverter
    {
        public void Convert(StreamWriter writer, StreamReader reader)
        {
            using (XmlTextWriter xmlWriter = new XmlTextWriter(writer))
            {
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.Indentation = 4;
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("coverage");
                xmlWriter.WriteAttributeString("version", "1");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
            }
        }
    }
}
