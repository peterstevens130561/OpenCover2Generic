namespace BHGE.SonarQube.OpenCover2Generic
{
    internal interface ICommandLineParser
    {
        string[] Args { get; set; }

        string GetArgument(string v);
    }
}