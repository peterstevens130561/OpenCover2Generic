using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Aggregates.Coverage
{
    public class CoverageAggregate : ICoverageAggregate
    {
        private string key;
        private string path;

        public CoverageAggregate(string path, string key)
        {
            this.path = path;
            this.key = key;
        }
    }
}
