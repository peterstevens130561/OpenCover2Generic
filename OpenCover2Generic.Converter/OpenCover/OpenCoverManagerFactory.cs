namespace BHGE.SonarQube.OpenCover2Generic.OpenCover
{
    public class OpenCoverManagerFactory : IOpenCoverManagerFactory
    {
        private readonly IOpenCoverProcessFactory _openCoverProcessFactory;


        public OpenCoverManagerFactory() : this(new OpenCoverProcessFactory())
        {
            
        }
        public OpenCoverManagerFactory(IOpenCoverProcessFactory openCoverProcessFactory)
        {
            _openCoverProcessFactory = openCoverProcessFactory;
        }

        public IOpenCoverManager CreateManager()
        {
            return new OpenCoverManager(_openCoverProcessFactory);
        }
    }
}
