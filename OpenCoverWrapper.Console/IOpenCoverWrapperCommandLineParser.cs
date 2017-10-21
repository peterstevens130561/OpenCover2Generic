namespace BHGE.SonarQube.OpenCoverWrapper
{
    internal interface IOpenCoverWrapperCommandLineParser
    {
        string[] Args { get; set; }

        string GetTargetPath();
        string GetTargetArgs();
        string GetOpenCoverPath();
    }
}