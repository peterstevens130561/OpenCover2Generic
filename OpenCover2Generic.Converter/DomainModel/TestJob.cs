using System.Collections.Generic;
using System.Linq;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

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

        public TestJob(IList<string> assemblies, string[] args, string repositoryRootDirectory,IWorkspace workspace) : this(assemblies)
        {
            Args = args;
            RepositoryRootDirectory = repositoryRootDirectory;
            Workspace = workspace;
        }

        public string FirstAssembly { get; private set; }

        public string Assemblies { get; private set;  }

        public string[] Args { get; private set; }

        public string RepositoryRootDirectory { get; private set; }

        public IWorkspace Workspace { get; private set; }
    }
}
