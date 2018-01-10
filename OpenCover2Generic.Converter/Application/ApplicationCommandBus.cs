using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Application.Commands.Workspace.Create;
using BHGE.SonarQube.OpenCover2Generic.Application.Commands.Workspace.Delete;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;

namespace BHGE.SonarQube.OpenCover2Generic.Application
{
    class ApplicationCommandBus : ICommandBus
    {
        private readonly CommandBus _commandBus;

        public ApplicationCommandBus() : this(new CommandBus(
            new CommandFactory()
                .Register<IWorkspaceCreateCommand,WorkspaceCreateCommand,WorkspaceCreateCommandHandler>()
                .Register<IWorkspaceDeleteCommand,WorkspaceDeleteCommand,WorkspaceDeleteCommandHandler>()
            )
        )
        {
            
        }

        public ApplicationCommandBus(CommandBus commandBus)
        {
            _commandBus = commandBus;

        }

        public TCommand CreateCommand<TCommand>() where TCommand : ICommand
        {
            return _commandBus.CreateCommand<TCommand>();
        }

        public void Execute<TCommand>(TCommand command) where TCommand : class,ICommand
        {
            _commandBus.Execute(command);
        }
    }
}
