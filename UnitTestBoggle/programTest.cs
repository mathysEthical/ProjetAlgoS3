using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ProgramNamespace;
namespace UnitTestBoggle
{
    [TestClass]
    public class programTest
    {
        [TestMethod]
        public void TreeContains()
        {
            string FRContent= Program.LoadFile("FR.txt");
            Assert.IsTrue(FRContent.Substring(0,10)== "TREMPES BR");
        }
    }
}
