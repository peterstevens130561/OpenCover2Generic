using BHGE.SonarQube.OpenCover2Generic.OpenCover;

namespace BHGE.SonarQube.OpenCover2Generic.Seams
{
    public interface IProcessFactory
    {
           IProcess CreateProcess();
    }
}