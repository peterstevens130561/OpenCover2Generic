using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    public class Jobs : IJobs
    {
        private readonly BlockingCollection<IJob> _jobs = new BlockingCollection<IJob>();
    }
}
