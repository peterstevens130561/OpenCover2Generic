

namespace BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus.Bus
{
    internal interface ICommandBus
    {
        T CreateCommand<T>() where T : ICommand;

        void Execute<T>(T command) where T : ICommand;

    }


}