using System;
using BHGE.SonarQube.OpenCover2Generic.Model;

namespace BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage
{
    public class ModuleEventArgs : EventArgs
    {

        public ModuleEventArgs(IModuleCoverageEntity entity)
        {
            Entity = entity;
        }

        public IModuleCoverageEntity Entity { get; private set; }
    }
}
