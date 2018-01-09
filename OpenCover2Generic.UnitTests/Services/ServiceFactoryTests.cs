using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Application.Commands;
using BHGE.SonarQube.OpenCover2Generic.Application.Services.Workspace;
using BHGE.SonarQube.OpenCover2Generic.CQRS.ServiceBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace;
namespace BHGE.SonarQube.OpenCover2Generic.Services
{
    [TestClass]
    public class ServiceFactoryTests
    {
        [TestInitialize]
        public void Initialize()
        {
            
        }

        [TestMethod]
        public void Execute_CreateWorkspace_Execute_EndsCorrect()
        {
            IServiceFactory serviceFactory=new ServiceFactory();
            var serviceBus = new ServiceBus(serviceFactory);
            serviceFactory.Register<IWorkspaceService, WorkspaceService, WorkspaceServiceHandler>();

            IWorkspaceService service = serviceBus.Create<IWorkspaceService>();
            service.Id = "bla";
            IWorkspace workspace = serviceBus.Execute(service);
            Assert.IsTrue(workspace.Path.EndsWith("opencover_bla"),workspace.Path);
        }



    }
}
