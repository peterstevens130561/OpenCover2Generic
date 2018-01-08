using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus.Bus;

namespace BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus.Factory
{
    internal interface ICommandFactory
    {
        T CreateCommand<T>() where T : ICommand;

        ICommandHandler<T> CreateHandler<T>(T command) where T : ICommand;

        void Register<TInterfaceType, TCommandType, THandlerType>() where TInterfaceType : ICommand
            where TCommandType : ICommand
            where THandlerType : ICommandHandler<TCommandType>;
    }


}