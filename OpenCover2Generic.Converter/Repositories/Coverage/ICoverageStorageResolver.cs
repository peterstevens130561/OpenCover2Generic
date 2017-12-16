using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public interface ICoverageStorageResolver
    {
        string GetPathForAssembly(string rootPath, string moduleName, string testAssemblyPath);
        IEnumerable<string> GetPathsOfAllModules(string rootPath);
        IEnumerable<string> GetTestCoverageFilesOfModule(string moduleDirectory);
    }
}
