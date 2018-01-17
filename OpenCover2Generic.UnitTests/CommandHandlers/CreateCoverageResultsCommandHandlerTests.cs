using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Application.Commands.CoverageResultsCreate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic.CommandHandlers
{
    [TestClass]
    public class CreateCoverageResultsCommandHandlerTests
    {
        private readonly CreateCoverageResultsCommandHandler _commandHandler;
        [TestMethod]
        public void Execute_Normal_Execute_DepsCalled()
        {
            ICreateCoverageResultsCommand command = new CreateCoverageResultsCommand();
  
        }
    }
}
