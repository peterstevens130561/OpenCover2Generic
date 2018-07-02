using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.CQRS.ServiceBus
{
    public interface IServiceHandler<out TResult,in TService>
    {
        TResult Execute(TService service);
    }
}
