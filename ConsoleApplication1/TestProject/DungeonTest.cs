using ConsoleApplication1;
using System.Collections.Generic;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class DungeonTest
    {
        [TestMethod]
        public void level_Test()
        {
            Dungeon dungeon = new Dungeon(3);
            foreach(OgNode node in dungeon.nodes)
            {
                Assert.AreEqual(node.nodeLevel, dungeon.level(node));
            }
        }

        [TestMethod]
        public void bridgeDestroy_Test()
        {
            Dungeon dungeon = new Dungeon(2);
            dungeon.BridgeDestroy(dungeon.bridges[2]);

            Assert.AreEqual(0, dungeon.zones[1].Count);
            Assert.AreEqual(0, dungeon.zones[2].Count);
            Assert.IsNull(dungeon.beginNode);

            Assert.AreEqual(1, dungeon.zones[3][0].neighbors.Count);
            Assert.AreEqual(1, dungeon.zones[3][1].neighbors.Count);

            OgNode neighbor1 = dungeon.zones[3][0].neighbors[0];
            OgNode neighbor2 = dungeon.zones[3][1].neighbors[0];

            Assert.AreSame(neighbor1, neighbor2);
            Assert.AreSame(neighbor1, dungeon.endNode);

            Assert.AreEqual(2, dungeon.endNode.neighbors.Count);
            if (!dungeon.endNode.neighbors.Contains(dungeon.zones[3][0]) || !dungeon.endNode.neighbors.Contains(dungeon.zones[3][1]))
                Assert.Fail();
        }

        [TestMethod]
        public void shortestPath_TestNullStartisEnd()
        {
            Dungeon dungeon = new Dungeon(1);
            List<OgNode> path = dungeon.shortestPath(dungeon.beginNode, dungeon.beginNode);
            Assert.IsNull(path);
        }

        [TestMethod]
        public void shortestPath_TestNullPathNotLegit()
        {
            Dungeon dungeon = new Dungeon(1);
            List<OgNode> path = dungeon.shortestPath(dungeon.beginNode, new Node(0, "I don't exist"));
            Assert.IsNull(path);
        }

        [TestMethod]
        public void shortestPath_TestNotNull()
        {
            Dungeon dungeon = new Dungeon(1);
            List<OgNode> path = dungeon.shortestPath(dungeon.bridges[1], dungeon.endNode);

            Assert.AreSame(path[0], dungeon.bridges[1]);
            Assert.AreSame(path[1], dungeon.zones[2][0]);
            Assert.AreSame(path[2], dungeon.endNode);
        }

        [TestMethod]
        public void DungeonGenerationTestWithBridges()
        {
            Dungeon dungeon = new Dungeon(3);

            Assert.AreEqual(3, dungeon.bridges.Length - 1);

            List<OgNode> path = dungeon.shortestPath(dungeon.beginNode, dungeon.endNode);
            int bridgeNumber = 0;
            foreach(OgNode node in path)
            {
                if (node.nodeLevel > 0) // Count number of bridges in shortest path from begin to end
                    bridgeNumber++;
            }

            Assert.AreEqual(3, bridgeNumber);
        }

        [TestMethod]
        public void DungeonGenerationTestWithoutBridges()
        {
            Dungeon dungeon = new Dungeon(0);

            Assert.AreEqual(0, dungeon.bridges.Length - 1);

            List<OgNode> path = dungeon.shortestPath(dungeon.beginNode, dungeon.endNode);
            int bridgeNumber = 0;
            foreach (OgNode node in path)
            {
                if (node.nodeLevel > 0) // Count number of bridges in shortest path from begin to end
                    bridgeNumber++;
            }

            Assert.AreEqual(0, bridgeNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void DungeonGenerationTest_NegativeBridges()
        {
            Dungeon dungeon = new Dungeon(-2);
        }

    }
}
