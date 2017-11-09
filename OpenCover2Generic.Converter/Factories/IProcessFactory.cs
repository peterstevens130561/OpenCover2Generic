using BHGE.SonarQube.OpenCover2Generic.Utils;
using System.Diagnostics;

namespace BHGE.SonarQube.OpenCover2Generic.Factories
{
    public interface IProcessFactory
    {
           IProcess CreateProcess();
    }
}