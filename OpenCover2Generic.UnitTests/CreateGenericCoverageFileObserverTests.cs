using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Repositories.Coverage;
using BHGE.SonarQube.OpenCover2Generic.Writers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    class CreateGenericCoverageFileObserverTests
    {
        private IGenericCoverageFileWriterObserver observer;
        [TestInitialize]
        public void Initialize()
        {
            observer = new GenericCoverageFileWriterObserver();
        }
    }
}
