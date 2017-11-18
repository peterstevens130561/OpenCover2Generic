using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Utils
{
    [Serializable]
    public class CommandLineArgumentException : InvalidOperationException

    {
        public CommandLineArgumentException(string message) : base(message)
        {
        }
    }
}
