using System;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public class ModuleEventArgs : EventArgs
    {

        public ModuleEventArgs(IModule entity)
        {
            Entity = entity;
        }

        public IModule Entity { get; private set; }
    }
}
