using BHGE.SonarQube.OpenCover2Generic.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic
{
    [TestClass]
    public class ModuleCoverageModelTests
    {
        private readonly IModuleCoverageEntity _entity = new ModuleCoverageEntity();


        [TestMethod]
        public void ModuleName_NotSet_ModuleName_Null()
        {
            Assert.IsNull(_entity.NameId);
        }

        [TestMethod]
        public void ModuleName_Set_ModuleName_EqualToSet()
        {
            _entity.NameId = "myname";
            Assert.AreEqual("myname",_entity.NameId);
        }
    }
}
