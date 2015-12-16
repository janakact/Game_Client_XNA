using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameClientXNA;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_G()
        {
            GameDetail game = new GameDetail();
            String msg = "G:P1;0,0;0;0;100;0;0:8,6,0;4,8,0;9,3,0;3,1,0;5,7,0#";
            game.processMsg(msg,new TimeSpan(0));
            //Assert.AreEqual(game.players[1].name, "P1");
            //Assert.AreEqual(game.,)
        }
    }
}
