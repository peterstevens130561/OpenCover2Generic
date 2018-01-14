namespace BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner
{
    internal interface IOpenCoverCommandLineParser
    {
        string[] Args { get; set; }

        string GetOutputPath();

    }
}