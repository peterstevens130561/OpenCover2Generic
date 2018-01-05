namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public interface IQueryAllModulesObservable
    {
        void Execute();
        IQueryAllModulesObservable AddObserver(IQueryAllModulesResultObserver queryAllModulesResultObserver);
    }
}
