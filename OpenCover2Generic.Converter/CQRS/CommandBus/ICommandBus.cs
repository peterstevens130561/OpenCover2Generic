

namespace BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus
{
    internal interface ICommandBus
    {
        T CreateCommand<T>() where T : ICommand;

        void Execute<T>(T command) where T : class,ICommand;



    }


}