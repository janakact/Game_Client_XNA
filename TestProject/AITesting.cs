using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameClientXNA;
using GameClientXNA.Game.AI;
using System.Collections.Generic;

namespace TestProject
{
    [TestClass]
    public class AITesting
    {
        [TestMethod]
        public void Test_GetPath_Method()
        {

            GameDetail game = new GameDetail();
            game.processMsg("I:P0:9,3;7,4;4,2;6,8;2,4;4,7;5,3;1,3;3,8;8,6:4,8;6,3;3,2;5,7;3,6;5,8;8,1;2,1;8,4;7,1:1,8;0,3;1,4;0,8;3,1;2,6;2,7;1,7;4,3;7,6#", new TimeSpan(0));

            List<int> path = TaskManager.getPath(game, 0, 2);

            
            Assert.AreEqual(0, 0);
            //Assert.AreEqual(game.,)
        }
    }
}
