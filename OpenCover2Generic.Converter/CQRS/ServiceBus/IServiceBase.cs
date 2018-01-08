namespace BHGE.SonarQube.OpenCover2Generic.CQRS.ServiceBus
{
    public interface IServiceBase<out TResult, in TService>
    {
        TResult Execute(TService service);
    }
}