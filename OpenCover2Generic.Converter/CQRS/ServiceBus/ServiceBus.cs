﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;
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
        public TService Create<TService>() 
        {
            return _serviceFactory.CreateService<TService>();
        }


        public TResult Execute<TResult,TService>(IServiceBase<TResult,TService> service)
        {
            IServiceHandler<TResult, TService> handler = _serviceFactory.CreateHandler<TResult, TService>((TService)service);
            return handler.Execute((TService)service);
        }
    }
}
