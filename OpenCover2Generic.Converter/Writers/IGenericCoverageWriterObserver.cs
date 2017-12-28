using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;

namespace BHGE.SonarQube.OpenCover2Generic.Writers
{
    public interface IGenericCoverageWriterObserver
    {
        XmlTextWriter Writer { get; set; }
    }
}
