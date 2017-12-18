namespace BHGE.SonarQube.OpenCover2Generic.OpenCover
{
    public class OpenCoverManagerFactory : IOpenCoverManagerFactory
    {
        private readonly IOpenCoverProcessFactory _openCoverProcessFactory;


        public OpenCoverManagerFactory(IOpenCoverProcessFactory openCoverProcessFactory)
        {
            _openCoverProcessFactory = openCoverProcessFactory;
        }

        public IOpenCoverRunnerManager CreateManager()
        {
            return new OpenCoverRunnerManager(_openCoverProcessFactory);
        }
    }
}
