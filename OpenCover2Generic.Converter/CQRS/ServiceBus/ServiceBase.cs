using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.CQRS.ServiceBus
{
    public class ServiceBase<TResult,TService> : IServiceBase<TResult,TService>,IService
    {
        protected readonly IServiceFactory ServiceFactory;

        public ServiceBase(IServiceFactory serviceFactory)
        {
            ServiceFactory = serviceFactory;
        }

    }
}
