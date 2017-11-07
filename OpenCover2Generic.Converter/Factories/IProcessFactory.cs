using System.Diagnostics;

namespace BHGE.SonarQube.OpenCover2Generic.Factories
{
    public interface IProcessFactory
    {
         Process CreateProcess();
    }
}