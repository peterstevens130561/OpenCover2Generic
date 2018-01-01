using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BHGE.SonarQube.OpenCover2Generic.Adapters
{
    public class XmlAdapter : IXmlAdapter
    {
        public XmlReader CreateReader(string path)
        {
            return XmlReader.Create(path);
        }
    }
}
