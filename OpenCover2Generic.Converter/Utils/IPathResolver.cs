namespace BHGE.SonarQube.OpenCover2Generic.Utils
{
    public interface IPathResolver
    {
        string Root { get; set; }
        string GetDirectory();
    }
}