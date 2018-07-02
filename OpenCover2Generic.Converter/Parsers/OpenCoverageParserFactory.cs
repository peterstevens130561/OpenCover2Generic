namespace BHGE.SonarQube.OpenCover2Generic.Parsers
{

    public class OpenCoverageParserFactory : IOpenCoverageParserFactory
    {

        public ICoverageParser Create()
        {
            return new OpenCoverCoverageParser();
        }
    }
}
