using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus.Factory;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus.Infrastructure;

namespace BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus.Bus
{
    class CommandBus : ICommandBus
    {
        private readonly ICommandFactory commandFactory;
        public CommandBus(ICommandFactory commandFactory) 
        {
            this.commandFactory = commandFactory;
  
        }

        public T CreateCommand<T>() where T : ICommand
        {
            return commandFactory.CreateCommand<T>();
        }

        public void Execute<T>(T command) where T : ICommand
        {
            var handler = commandFactory.CreateHandler(command);
            handler.Execute(command);
        }
    }
    }
