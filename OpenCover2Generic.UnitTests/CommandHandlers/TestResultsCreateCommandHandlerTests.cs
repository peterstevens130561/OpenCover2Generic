using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Application;
using BHGE.SonarQube.OpenCover2Generic.Application.Commands.TestResultsCreate;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Tests;
using BHGE.SonarQube.OpenCoverWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BHGE.SonarQube.OpenCover2Generic.CommandHandlers
{
    [TestClass]
    public class TestResultsCreateCommandHandlerTests
    {
        private TestResultsCreateCommandHandler testResultsCreateCommandHandler;
        private ITestResultsCreateCommand command;
        private Mock<ITestResultsRepository> testResultsRepositoryMock;
        private Mock<IOpenCoverWrapperCommandLineParser> commandLineParserMock;

        [TestInitialize]
        public void Initialize()
        {
            testResultsRepositoryMock=new Mock<ITestResultsRepository>();
            commandLineParserMock=new Mock<IOpenCoverWrapperCommandLineParser>();
            testResultsCreateCommandHandler=new TestResultsCreateCommandHandler(
                testResultsRepositoryMock.Object,
                commandLineParserMock.Object);
            command = new TestResultsCreateCommand();
        }

        [TestMethod]
        public void Execute_ValidPath_Execute_WriteToPath()
        {
            string[] args = new[] {"a", "b"};
            command.Args = args;
            IWorkspace workspace = new Workspace("path");
            command.Workspace = workspace;
            commandLineParserMock.Setup(c => c.GetTestResultsPath()).Returns("resultspath");
            testResultsCreateCommandHandler.Execute(command);

            commandLineParserMock.VerifySet(c => c.Args=args,Times.Once);
            testResultsRepositoryMock.Verify( t => t.SetWorkspace(workspace));
            testResultsRepositoryMock.Verify(t => t.Write("resultspath"),Times.Once);
        }

        [TestMethod]
        public void Create_CommandBus_Create_GetCommand()
        {
            ICommandBus commandBus = new ApplicationCommandBus();
            ITestResultsCreateCommand command = commandBus.CreateCommand<ITestResultsCreateCommand>();
            Assert.IsNotNull(command);
        }

    }
}
