using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHGE.SonarQube.OpenCover2Generic.Application.Commands.RunTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BHGE.SonarQube.OpenCover2Generic.CommandHandlers
{
    [TestClass]
    public class TestRunnerCommandTests
    {
        private TestRunnerCommand _command;

        [TestInitialize]
        public void Initialize()
        {
            _command = new TestRunnerCommand();
        }
        [TestMethod]
        public void Args_ValueSet_Get_GetSameValue()
        {
            string[] value = new[] {"a"};
            _command.Args = value;
            Assert.AreEqual(_command.Args,value);
        }
    }
}
