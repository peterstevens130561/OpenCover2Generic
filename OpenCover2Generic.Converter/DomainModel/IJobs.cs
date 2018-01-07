namespace BHGE.SonarQube.OpenCover2Generic.DomainModel
{
    public interface IJobs
    {
        void Add(ITestJob testJob);

        ITestJob Take();
        void CompleteAdding();

        bool IsCompleted();

        int Count();
    }
}