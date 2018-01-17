using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.CoverageResultsCreate
{
    class CreateCoverageResultsCommandHandler : ICommandHandler<ICreateCoverageResultsCommand>
    {
        public void Execute(ICreateCoverageResultsCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
