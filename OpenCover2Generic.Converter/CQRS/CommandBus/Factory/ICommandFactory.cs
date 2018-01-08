using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus.Infrastructure;

namespace BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus.Factory
{
    internal interface ICommandFactory
    {
        T CreateCommand<T>() where T : ICommand;

        ICommandHandler<T> CreateHandler<T>(T command) where T : ICommand;

    }


}