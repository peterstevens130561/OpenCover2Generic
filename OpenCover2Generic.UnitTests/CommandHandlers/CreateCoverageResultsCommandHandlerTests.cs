using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Adapters;
using BHGE.SonarQube.OpenCover2Generic.Application;
using BHGE.SonarQube.OpenCover2Generic.Application.Commands.CoverageResultsCreate;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Writers;
using BHGE.SonarQube.OpenCoverWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BHGE.SonarQube.OpenCover2Generic.CommandHandlers
{
    [TestClass]
    public class CreateCoverageResultsCommandHandlerTests
    {
        private CreateCoverageResultsCommandHandler _commandHandler;
        private Mock<ICodeCoverageRepository> _repositoryMock;

        [TestMethod]
        public void Execute_Normal_Execute_DepsCalled()
        {
            ICreateCoverageResultsCommand command = new CreateCoverageResultsCommand();
            _repositoryMock = new Mock<ICodeCoverageRepository>();

            command.Args = new[] {"-output:opencover.xml"};
            command.Workspace = new Workspace("jadieda");

            IOpenCoverWrapperCommandLineParser commandLineParser = new OpenCoverWrapperCommandLineParser();

            var queryMock = new Mock<IQueryAllModulesObservable>();
            var genericWriterObserverMock = new Mock<IGenericCoverageWriterObserver>();
            IGenericCoverageWriterObserver genericCoverageWriterObserver = genericWriterObserverMock.Object;

            var statisticsObserverMock = new Mock<ICoverageStatisticsAggregator>();
            ICoverageStatisticsAggregator statisticsObserver = statisticsObserverMock.Object;

            var codeCoverageRepositoryMock = new Mock<ICodeCoverageRepository>();
            ICodeCoverageRepository codeCoverageRepository = codeCoverageRepositoryMock.Object;
            codeCoverageRepositoryMock.Setup(c => c.QueryAllModules()).Returns(queryMock.Object);
            queryMock.Setup(q => q.AddObserver(It.IsAny<IQueryAllModulesResultObserver>())).Returns(queryMock.Object);

            var xmlAdapterMock = new Mock<IXmlAdapter>();

            _commandHandler = new CreateCoverageResultsCommandHandler(
                commandLineParser,
                codeCoverageRepository,
                genericCoverageWriterObserver,
                statisticsObserver,
                xmlAdapterMock.Object);

            _commandHandler.Execute(command);
            xmlAdapterMock.Verify(x => x.CreateTextWriter("opencover.xml"), Times.Once);
            codeCoverageRepositoryMock.Verify(c => c.QueryAllModules(), Times.Once);
            queryMock.Verify(q => q.AddObserver(It.IsAny<IQueryAllModulesResultObserver>()), Times.Exactly(2));
            queryMock.Verify(q => q.Execute(), Times.Exactly(1));
        }

        [TestMethod]
        public void Create_Command()
        {
            ICommandBus commandBus = new ApplicationCommandBus();
            ICreateCoverageResultsCommand command=commandBus.CreateCommand<ICreateCoverageResultsCommand>();
            command.Args = new[] { "-output:opencover.xml" };
            command.Workspace = new Workspace("jadieda");
            Assert.AreEqual("-output:opencover.xml",command.Args[0]);
            Assert.AreEqual("jadieda",command.Workspace.Path);

        }
    }
}
