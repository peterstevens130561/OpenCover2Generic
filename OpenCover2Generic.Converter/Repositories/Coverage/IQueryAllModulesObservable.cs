using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public interface IQueryAllModulesObservable
    {
        void Execute();
        IQueryAllModulesObservable AddObserver(IQueryAllModulesResultObserver queryAllModulesResultObserver);
    }
}
