namespace BHGE.SonarQube.OpenCover2Generic.OpenCoverRunner
{
    internal interface IOpenCoverWrapperCommandLineParser
    {
        string[] Args { get; set; }

        string GetTargetPath();
        string GetTargetArgs();
    }
}