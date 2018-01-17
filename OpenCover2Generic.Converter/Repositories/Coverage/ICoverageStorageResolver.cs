using System.Collections.Generic;
using BHGE.SonarQube.OpenCover2Generic.Utils;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public interface ICoverageStorageResolver : IPathResolver
    {
        string GetPathForAssembly(string rootPath, string moduleName, string testAssemblyPath);
        IEnumerable<string> GetPathsOfAllModules(string rootPath);
        IEnumerable<string> GetTestCoverageFilesOfModule(string moduleDirectory);
        string GetPathForAssembly(string moduleName, string testAssemblyPath);
        IEnumerable<string> GetPathsOfAllModules();
    }
}
