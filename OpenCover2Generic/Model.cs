using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic
{
    class Model : IModel
    {
        readonly IDictionary<string, string> dictionary = new Dictionary<string, string>();

        public void AddFile(string fileId, string filePath)
        {
            dictionary.Add(fileId, filePath);
        }


    }
}
