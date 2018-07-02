namespace BHGE.SonarQube.OpenCover2Generic.Parsers
{
    public interface IOpenCoverageParserFactory
    {
        ICoverageParser Create();
    }
    
}