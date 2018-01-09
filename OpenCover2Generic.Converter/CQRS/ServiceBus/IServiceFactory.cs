
namespace BHGE.SonarQube.OpenCover2Generic.CQRS.ServiceBus
{
    public interface IServiceFactory
    {
        TService CreateService<TService>();
        IServiceHandler<TResult,TService> CreateHandler<TResult,TService>(TService service);
        void Register<TServiceInterface, TServiceImplementation, TServiceHandler>();
    }
}