using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.CQRS.CommandBus;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Commands.Tests
{
    public class RunTestsCommandHandler : ICommandHandler<IRunTestsCommand>
    {
        public void Execute(IRunTestsCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
