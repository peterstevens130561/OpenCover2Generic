using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Application.Commands.Tests;
using BHGE.SonarQube.OpenCover2Generic.Application.Commands.Workspace.Create;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic.CommandHandlers
{
    [TestClass]
    class RunTestsCommandTests
    {
        private  ICommandBus _commandBus;

        [TestInitialize]
        public void Initialize()
        {
            ICommandFactory commandFactory = new CommandFactory();
            _commandBus = new CommandBus(commandFactory);
            commandFactory.Register<IRunTestsCommand, RunTestsCommand, RunTestsCommandHandler>();
        }
        [TestMethod]
        public void RunTests_NoAssemblies_RunTests_CompletesFine()
        {
            IRunTestsCommand runTestsCommand = _commandBus.CreateCommand<IRunTestsCommand>();
            _commandBus.Execute(runTestsCommand);

        }
    }
}
