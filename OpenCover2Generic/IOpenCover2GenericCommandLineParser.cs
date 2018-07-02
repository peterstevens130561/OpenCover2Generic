namespace BHGE.SonarQube.OpenCover2Generic
{
    interface IOpenCover2GenericCommandLineParser 
    {
        string[] Args { get; set; }

        string OpenCoverPath();
        string GenericPath();
    }
}
