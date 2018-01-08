using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus.Factory;

namespace BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus.Bus
{
    class CommandBus : ICommandBus
    {
        private readonly ICommandFactory _commandFactory;
        public CommandBus(ICommandFactory commandFactory) 
        {
            this._commandFactory = commandFactory;
  
        }

        public T CreateCommand<T>() where T : ICommand
        {
            return _commandFactory.CreateCommand<T>();
        }

        public void Execute<T>(T command) where T : ICommand
        {
            var handler = _commandFactory.CreateHandler(command);
            handler.Execute(command);
        }
    }
    }
