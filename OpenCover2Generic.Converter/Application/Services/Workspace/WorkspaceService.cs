﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Commands;
using BHGE.SonarQube.OpenCover2Generic.CQRS.ServiceBus;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;

namespace BHGE.SonarQube.OpenCover2Generic.Application.Services.Workspace
{
    class WorkspaceService<TService> : ServiceBase<IWorkspace,IWorkspaceService>, IService
    {
        public WorkspaceService(IServiceFactory serviceFactory) : base(serviceFactory)
        {
            
        }
        public string Id { get;set; }
    }
}
