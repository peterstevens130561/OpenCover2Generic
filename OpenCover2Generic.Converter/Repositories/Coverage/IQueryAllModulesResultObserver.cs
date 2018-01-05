using System;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public interface IQueryAllModulesResultObserver
    {
        void OnBeginScan(object sender, EventArgs eventArgs);
        void OnEndScan(object sender, EventArgs eventArgs);
        void OnModule(object v, ModuleEventArgs moduleEventArgs);
    }
}
