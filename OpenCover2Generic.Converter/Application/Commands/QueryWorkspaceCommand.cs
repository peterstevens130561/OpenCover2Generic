using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.CQRS.ServiceBus;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Commands
{
    class WorkspaceService : ServiceBase<IWorkspace, IWorkspaceService>,IWorkspaceService
    {
        public WorkspaceService(IServiceFactory serviceFactory) : base(serviceFactory)
        {
        }

        public string Id { get; set; }
    }
}
