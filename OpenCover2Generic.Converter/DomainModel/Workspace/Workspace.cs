using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.DomainModel.Workspace
{
    public class Workspace : IWorkspace
    {
        internal Workspace(string path)
        {
            Path = path;
        }

        public string Path { get; private set; }
    }
}
