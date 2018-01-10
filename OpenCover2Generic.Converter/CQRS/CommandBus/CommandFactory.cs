using System;
using System.Collections.Generic;

namespace BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus
{
    class CommandFactory : ICommandFactory
    {
        public CommandFactory()
        {
                     
        }

        private readonly Dictionary<Type, Type> _commandMap = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, Type> _handlerMap = new Dictionary<Type, Type>();

        public ICommandFactory Register<TCommandInterface, TCommandImplementation, TCommandHandler>()
            where TCommandInterface : ICommand
            where TCommandImplementation : class,ICommand
            where TCommandHandler : class,ICommandHandler<TCommandImplementation>
        {
            _commandMap.Add(typeof(TCommandInterface), typeof(TCommandImplementation));
            _handlerMap.Add(typeof(TCommandInterface), typeof(TCommandHandler));
            return this;
        }

        public T CreateCommand<T>() where T : ICommand
        {
            if (!_commandMap.ContainsKey(typeof(T)))
            {
                throw new ArgumentException(@"unsupported type");
            }
            var concrete = _commandMap[typeof(T)];
            return (T)Activator.CreateInstance(concrete);

        }

        public ICommandHandler<TCommand> CreateHandler<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (!_handlerMap.ContainsKey(typeof(TCommand)))
            {
                throw new ArgumentException($"unsupported type {command.GetType().Name}");
            }
            var handlerType = _handlerMap[typeof(TCommand)];
            return (ICommandHandler<TCommand>)Activator.CreateInstance(handlerType);

        }

    }
}
