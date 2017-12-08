using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories
{
    public interface ICoverageStorageResolver
    {
        string GetPathForAssembly(string rootPath, string moduleName, string testAssemblyPath);
        IEnumerable<string> GetPathsOfAllModules(string rootPath);
        IEnumerable<string> GetTestCoverageFilesOfModule(string moduleDirectory);
    }
}
