using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Application.Commands.CoverageResultsCreate;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
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
            _repositoryMock=new Mock<ICodeCoverageRepository>();

            command.Args = new[] {"-output:opencover.xml"};
            command.Workspace = new Workspace("jadieda");
            _commandHandler = new CreateCoverageResultsCommandHandler();
           
        }
    }
}
