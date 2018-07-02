using System.Collections.Generic;
using BHGE.SonarQube.OpenCover2Generic.Utils;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public interface ICoverageRepositoryPathResolver : IPathResolver
    {

        IEnumerable<string> GetTestCoverageFilesOfModule(string moduleDirectory);
        string GetPathForAssembly(string moduleName, string testAssemblyPath);
        IEnumerable<string> GetPathsOfAllModules();
    }
}
