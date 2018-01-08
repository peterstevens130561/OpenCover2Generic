using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.CQRS.ServiceBus
{
    public class ServiceBase<TResult,TService> : IServiceBase<TResult,TService>,IService
    {
        private readonly IServiceFactory _serviceFactory;

        public ServiceBase(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }
        public TResult Execute(TService service)
        {
            IServiceHandler<TResult,TService> handler = _serviceFactory.CreateHandler<TResult,TService>(service);
            return handler.Execute(service);
        }
    }
}
