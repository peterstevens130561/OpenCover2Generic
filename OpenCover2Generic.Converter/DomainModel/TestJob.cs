using System.Collections.Generic;
using System.Linq;

namespace BHGE.SonarQube.OpenCover2Generic.DomainModel
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

        public TestJob(IList<string> assemblies, string[] args, string repositoryRootDirectory) : this(assemblies)
        {
            Args = args;
            RepositoryRootDirectory = repositoryRootDirectory;
        }

        public string FirstAssembly { get; private set; }

        public string Assemblies { get; private set;  }

        public string[] Args { get; private set; }

        public string RepositoryRootDirectory { get; private set; }
    }
}
