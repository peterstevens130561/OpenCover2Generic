using BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner;

namespace BHGE.SonarQube.OpenCover2Generic.Factories
{
    public interface IOpenCoverManagerFactory
    {
        IOpenCoverRunnerManager CreateManager();
    }
}