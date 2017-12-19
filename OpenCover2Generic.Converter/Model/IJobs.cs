namespace BHGE.SonarQube.OpenCover2Generic.Model
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