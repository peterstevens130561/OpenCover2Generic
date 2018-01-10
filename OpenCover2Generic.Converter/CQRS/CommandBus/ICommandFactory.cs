namespace BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus
{
    internal interface ICommandFactory
    {
        T CreateCommand<T>() where T : ICommand;

        ICommandHandler<T> CreateHandler<T>(T command) where T : class,ICommand;

        ICommandFactory Register<TInterfaceType, TCommandType, THandlerType>() where TInterfaceType : ICommand
            where TCommandType : class,ICommand
            where THandlerType : class,ICommandHandler<TCommandType>;
    }


}