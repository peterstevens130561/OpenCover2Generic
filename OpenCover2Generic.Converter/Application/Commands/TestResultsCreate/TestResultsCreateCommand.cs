using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.TestResultsCreate
{
    public class TestResultsCreateCommand : ITestResultsCreateCommand
    {
        public string[] Args { get; set; }

        public IWorkspace Workspace { get; set; }
    }
}
