namespace BHGE.SonarQube.OpenCover2Generic.CQRS.ServiceBus
{
    public interface IServiceBus
    {
        TService Create<TService>();
        TResult Execute<TResult, TService>(IServiceBase<TResult, TService> service);
    }
}