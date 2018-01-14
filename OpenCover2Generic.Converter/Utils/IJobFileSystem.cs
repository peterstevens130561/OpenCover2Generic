using System.Collections.Generic;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Utils
{
    public interface IJobFileSystem
    {
        string GetOpenCoverLogPath(string assembly);
        string GetOpenCoverOutputPath(string assembly);
        string GetTestResultsPath(string assembly);
        string GetIntermediateCoverageDirectory();
        string GetTestResultsDirectory();

        IEnumerable<string> GetTestResultsFiles();
        void CreateRoot(IWorkspace workspace);

        IEnumerable<string> GetModuleCoverageDirectories();

        IEnumerable<string> GetTestCoverageFilesOfModule(string moduleDirectory);
    }
}