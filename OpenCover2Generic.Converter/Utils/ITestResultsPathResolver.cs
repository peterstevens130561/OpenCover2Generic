using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Adapters;

namespace BHGE.SonarQube.OpenCover2Generic.Utils
{
    public interface ITestResultsPathResolver
    {
        string Root { get; set; }

        string GetDirectory();
        string GetTestResultsDestinationPath(string v);

        IEnumerable<string> GetTestResultsFiles();
    }

}
