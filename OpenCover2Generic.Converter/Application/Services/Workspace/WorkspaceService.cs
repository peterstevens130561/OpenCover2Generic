using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.CQRS.ServiceBus;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Services.Workspace
{
    class WorkspaceService: IWorkspaceService
    {
        public WorkspaceService() 
        {
            
        }
        public string Id { get;set; }
    }
}
