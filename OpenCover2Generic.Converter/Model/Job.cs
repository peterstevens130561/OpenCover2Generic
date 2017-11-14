using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public class Job : IJob
    {
        public Job(List<string> assemblies)
        {
            FirstAssembly = assemblies.First();
            Assemblies = string.Join(" ", assemblies);
        }

        public Job(string assembly)
        {
            FirstAssembly = assembly;
            Assemblies = assembly;
        }

        public string FirstAssembly { get; private set; }

        public string Assemblies { get; private set;  }

    }
}
