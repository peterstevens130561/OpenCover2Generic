namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public interface IJobs
    {
        void Add(IJob job);

        IJob Take();
        void CompleteAdding();

        bool IsCompleted();

        int Count();
    }
}