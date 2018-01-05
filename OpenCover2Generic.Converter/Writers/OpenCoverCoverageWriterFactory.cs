namespace BHGE.SonarQube.OpenCover2Generic.Writers
{
    public class CoverageWriterFactory : ICoverageWriterFactory
    {
        public ICoverageWriter CreateOpenCoverCoverageWriter()
        {
            return new OpenCoverCoverageWriter();
        }
    }
}
