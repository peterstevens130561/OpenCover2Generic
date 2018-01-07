namespace BHGE.SonarQube.OpenCover2Generic.DomainModel
{
    public interface ITestJob
    {
        string Assemblies { get; }
        string FirstAssembly { get; }
    }
}