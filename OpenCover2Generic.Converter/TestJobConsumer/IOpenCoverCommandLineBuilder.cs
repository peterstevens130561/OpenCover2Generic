using System.Diagnostics;

namespace BHGE.SonarQube.OpenCover2Generic.Utils
{
    public interface IOpenCoverCommandLineBuilder
    {
        string[] Args { get; set; }

        /// <summary>
        /// Creates a process startinfo for OpenCover, parsing the Args provided for relevant
        /// arguments, adding outputPath.
        /// assemblyPath is added in the targetargs
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        ProcessStartInfo Build(string assemblyPath, string outputPath);
    }
}