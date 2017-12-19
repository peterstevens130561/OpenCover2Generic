namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public interface ITestJob
    {
        string Assemblies { get; }
        string FirstAssembly { get; }
    }
}