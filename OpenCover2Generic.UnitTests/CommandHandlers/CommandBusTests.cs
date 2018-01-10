using System;
using BHGE.SonarQube.OpenCover2Generic.Application.Commands.Workspace.Create;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic.CommandHandlers
{
    [TestClass]
    public class CommandBusTests
    {
        private bool called;
        [TestInitialize]
        public void TestInitialize()
        {
            
        }

        [TestMethod]
        public void CommandFactoryCreate_ValidCommand_CreateCommand_ValidCommand()
        {
            ICommandFactory commandFactory = new CommandFactory();
            var commandBus = new CommandBus(commandFactory);
            commandFactory.Register<IWorkspaceCreateCommand, WorkspaceCreateCommand, WorkspaceCreateCommandHandler>();
            IWorkspaceCreateCommand command = commandFactory.CreateCommand<IWorkspaceCreateCommand>();
            Assert.IsNotNull(command);
        }

        [TestMethod]
        public void CommandFactoryCreate_InvalidValidCommand_CreateCommand_Exception()
        {
            ICommandFactory commandFactory = new CommandFactory();
            var commandBus = new CommandBus(commandFactory);
            commandFactory.Register<IWorkspaceCreateCommand, IWorkspaceCreateCommand, WorkspaceCreateCommandHandler>();
            // note: the if the user would use IBogusCommand, then the compiler will already block you
            try
            {
                var command = commandFactory.CreateCommand<ISomeBogusCommand>();
            }
            catch (ArgumentException)
            {
                return;
            }
            Assert.Fail("expected exception, as the interface is not registered");
        }

        private interface ISomeBogusCommand : ICommand
        {
            
        }

        [TestMethod]
        public void CommandBusCreate_ValidCommand_CreateCommand_ValidCommand()
        {
            ICommandFactory commandFactory = new CommandFactory();
            commandFactory.Register<IWorkspaceCreateCommand, WorkspaceCreateCommand, WorkspaceCreateCommandHandlerStub>();
            var commandBus = new CommandBus(commandFactory);
            var command = commandBus.CreateCommand<IWorkspaceCreateCommand>();
            command.Workspace = new Workspace("bla");
            commandBus.Execute(command);
        }

        private class WorkspaceCreateCommandHandlerStub : ICommandHandler<IWorkspaceCreateCommand>
        {
            public void Execute(IWorkspaceCreateCommand command)
            {
                Assert.IsTrue(command.Workspace.Path.EndsWith("bla"), command.Workspace.Path);
            }
        }
    }

}
