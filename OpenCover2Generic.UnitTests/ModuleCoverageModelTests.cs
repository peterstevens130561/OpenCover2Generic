using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class ModuleCoverageModelTests
    {
        private readonly IModuleCoverageModel _model = new ModuleCoverageModel();
        [TestInitialize]
        public void Initialize()
        {
            
        }

        [TestMethod]
        public void ModuleName_NotSet_ModuleName_Null()
        {
            Assert.IsNull(_model.Name);
        }

        [TestMethod]
        public void ModuleName_Set_ModuleName_EqualToSet()
        {
            _model.Name = "myname";
            Assert.AreEqual("myname",_model.Name);
        }
    }
}
