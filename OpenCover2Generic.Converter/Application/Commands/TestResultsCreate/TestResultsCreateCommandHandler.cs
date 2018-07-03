using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Tests;
using BHGE.SonarQube.OpenCoverWrapper;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.TestResultsCreate
{
    class TestResultsCreateCommandHandler : ICommandHandler<ITestResultsCreateCommand>
    {
        private readonly ITestResultsRepository _testResultsRepository;
        private readonly IOpenCoverWrapperCommandLineParser _commandLineParser;

        public TestResultsCreateCommandHandler() : this(
            new TestResultsRepository(),
            new OpenCoverWrapperCommandLineParser())
        {
            
        }

        public TestResultsCreateCommandHandler(
            ITestResultsRepository testResultsRepository, 
            IOpenCoverWrapperCommandLineParser commandLineParser)
        {
            _testResultsRepository = testResultsRepository;
            _commandLineParser = commandLineParser;
        }

        public void Execute(ITestResultsCreateCommand command)
        {
            _commandLineParser.Args = command.Args;
            _testResultsRepository.SetWorkspace(command.Workspace);

            string testResultsPath = _commandLineParser.GetTestResultsPath();
            _testResultsRepository.Write(testResultsPath);
        }
    }
}
