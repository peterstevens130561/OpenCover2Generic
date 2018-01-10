using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Application.Services.Workspace;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;
using BHGE.SonarQube.OpenCover2Generic.CQRS.ServiceBus;

namespace BHGE.SonarQube.OpenCover2Generic.Application
{
    public class ApplicationServiceBus : IServiceBus
    {
        private readonly IServiceBus _serviceBus;

        public ApplicationServiceBus(): this(
            new ServiceBus(
                new ServiceFactory()
            .Register<IWorkspaceService,WorkspaceService,WorkspaceServiceHandler>()
            ))
        {
            
        }

        public ApplicationServiceBus(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        public TService Create<TService>()
        {
            return _serviceBus.Create<TService>();
        }

        public TResult Execute<TResult, TService>(IServiceBase<TResult, TService> service)
        {
            return _serviceBus.Execute<TResult, TService>(service);
        }
    }
}
