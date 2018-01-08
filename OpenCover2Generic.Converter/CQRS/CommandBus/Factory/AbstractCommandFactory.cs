using System;
using System.Collections.Generic;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus.Infrastructure;

namespace BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus.Factory
{
    class AbstractCommandFactory : ICommandFactory
    {
        private readonly Dictionary<Type, Type> commandMap = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, Type> handlerMap = new Dictionary<Type, Type>();


        public AbstractCommandFactory()
        {
        }
        protected void Register<interfaceType, commandType, handlerType>()
            where interfaceType : ICommand
            where commandType : ICommand
            where handlerType : ICommandHandler<commandType>
        {
            commandMap.Add(typeof(interfaceType), typeof(commandType));
            handlerMap.Add(typeof(commandType), typeof(handlerType));
        }

        public T CreateCommand<T>() where T : ICommand
        {
            if (!commandMap.ContainsKey(typeof(T)))
            {
                throw new ArgumentException(@"unsupported type");
            }
            var concrete = commandMap[typeof(T)];
            return (T)Activator.CreateInstance(concrete);

        }

        public ICommandHandler<TCommand> CreateHandler<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (!handlerMap.ContainsKey(typeof(TCommand)))
            {
                throw new ArgumentException(@"unsupported type");
            }
            var handlerType = handlerMap[typeof(TCommand)];
            return (ICommandHandler<TCommand>)Activator.CreateInstance(handlerType);

        }
    }
}
