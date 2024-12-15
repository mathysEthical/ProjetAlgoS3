using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TreeNamespace;
namespace UnitTestBoggle
{
    [TestClass]
    public class treeTest
    {
        [TestMethod]
        public void TreeContains()
        {
            Tree testTree=new Tree();
            testTree.AddWord("CHOCOLAT");
            Assert.IsTrue(testTree.Contains("CHOCOLAT"));
        }

        [TestMethod]
        public void TreeNotContains()
        {
            Tree testTree = new Tree();
            testTree.AddWord("CHOCOLAT");
            Assert.IsFalse(testTree.Contains("MAISON"));
        }

        [TestMethod]
        public void TreeAnyStartingWith()
        {
            Tree testTree = new Tree();
            testTree.AddWord("CHOCOLAT");
            Assert.IsTrue(testTree.anyStartingWith("CHOCO"));
        }
    }
}
