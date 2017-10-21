namespace BHGE.SonarQube.OpenCover2Generic
{
    public interface ICommandLineParser
    {
        string[] Args { get; set; }

        string GetArgument(string v);
    }
}