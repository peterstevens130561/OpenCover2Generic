using BHGE.SonarQube.OpenCover2Generic.Seams;

namespace BHGE.SonarQube.OpenCover2Generic.OpenCover
{
    public class OpenCoverManagerFactory : IOpenCoverManagerFactory
    {

        private readonly IProcessFactory _processFactory;

        public OpenCoverManagerFactory(IProcessFactory processFactory)
        {
            _processFactory = processFactory;
        }

        public IOpenCoverRunnerManager CreateManager()
        {
            return new OpenCoverRunnerManager(_processFactory);
        }
    }
}
