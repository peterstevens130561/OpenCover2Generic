using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public class Job : IJob
    {
        public Job(string assembly)
        {
            Assembly = assembly;
        }

        public string Assembly { get; private set; }
    }
}
