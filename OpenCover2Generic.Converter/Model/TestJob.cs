using System.Collections.Generic;
using System.Linq;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public class TestJob : ITestJob
    {
        public TestJob(IList<string> assemblies)
        {
            FirstAssembly = assemblies.First();
            Assemblies = string.Join(" ", assemblies);
        }

        public TestJob(string assembly)
        {
            FirstAssembly = assembly;
            Assemblies = assembly;
        }

        public string FirstAssembly { get; private set; }

        public string Assemblies { get; private set;  }
    }
}
