using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Tests;

namespace BHGE.SonarQube.OpenCover2Generic.CQRS.ServiceBus
{
    public class ServiceBus : IServiceBus
    {
        private IServiceFactory _serviceFactory;

        public ServiceBus(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }
        public TService Create<TService>() where TService : IService
        {
            return _serviceFactory.CreateService<TService>();
        }

  

    }
}
