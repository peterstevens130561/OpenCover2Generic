using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.CoverageResultsCreate
{
    class CreateCoverageResultsCommand : ICreateCoverageResultsCommand
    {
        public IWorkspace Workspace { get; set; }
        public string[] Args { get; set; }
    }
}
