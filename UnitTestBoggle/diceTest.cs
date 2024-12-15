using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DiceNamespace;
namespace UnitTestBoggle
{
    [TestClass]
    public class diceTest
    {
        [TestMethod]
        public void diceRoll()
        {
            Dice dice = new Dice(new Random(), new char[] { 'A'}, new int[] { 1});
            Assert.AreEqual(dice.Roll(),'A');
        }
        public void diceToString()
        {
            Dice dice = new Dice(new Random(), new char[] { 'A' }, new int[] { 1 });
            Assert.AreEqual(dice.ToString(), "A");
        }
    }
}
