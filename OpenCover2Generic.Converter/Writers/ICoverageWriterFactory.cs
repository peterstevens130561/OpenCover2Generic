namespace BHGE.SonarQube.OpenCover2Generic.Writers
{
    public interface ICoverageWriterFactory
    {
        ICoverageWriter CreateOpenCoverCoverageWriter();
    }
}