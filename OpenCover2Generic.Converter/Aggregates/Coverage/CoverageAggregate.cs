using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage
{
    public class CoverageAggregate : ICoverageAggregate
    {

        public CoverageAggregate(string path, string key)
        {
            Path = path;
            Key = key;
        }

        public string Key { get; private set; }
        public string Path { get; private set; }
    }
}
