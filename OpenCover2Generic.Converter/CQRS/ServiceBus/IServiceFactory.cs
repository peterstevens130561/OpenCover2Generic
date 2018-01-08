using System.Runtime.Remoting;

namespace BHGE.SonarQube.OpenCover2Generic.CQRS.ServiceBus
{
    public interface IServiceFactory
    {
        TService CreateService<TService>() where TService : IService;
        IServiceHandler<TResult,TService> CreateHandler<TResult,TService>(TService service);
        void Register<TServiceInterface, TServiceImplementation, TServiceHandler>() where TServiceInterface : IService;
    }
}