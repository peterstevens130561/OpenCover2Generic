using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic
{
    interface IOpenCover2GenericCommandLineParser 
    {
        string[] Args { get; set; }

        string OpenCoverPath();
        string GenericPath();
    }
}
