using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.CQRS.ServiceBus
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly Dictionary<Type, Type> _serviceMap = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, Type> _handlerMap = new Dictionary<Type, Type>();
        public IServiceHandler<TResult, TService> CreateHandler<TResult, TService>(TService service)
        {
            var handler = _handlerMap[typeof(TService)];
            return (IServiceHandler<TResult,TService>)Activator.CreateInstance(handler);
        }

        public TService CreateService<TService>() 
        {
            var implementation = _serviceMap[typeof(TService)];
            return (TService)Activator.CreateInstance(implementation);
        }

        public IServiceFactory Register<TServiceInterface, TServiceImplementation, TServiceHandler>()
        {
            _serviceMap.Add(typeof(TServiceInterface), typeof(TServiceImplementation));
            _handlerMap.Add(typeof(TServiceInterface), typeof(TServiceHandler));
            return this;
        }
    }
}
