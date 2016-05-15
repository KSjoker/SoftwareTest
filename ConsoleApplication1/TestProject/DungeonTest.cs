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
            dungeon.BridgeDestroy(dungeon.bridges[2]); //Destroy bridge 2

            Assert.AreEqual(0, dungeon.zones[1].Count); //Zone 1 should be empty now
            Assert.AreEqual(0, dungeon.zones[2].Count); //Zone 2 should b empty now
            Assert.IsNull(dungeon.beginNode);

            Assert.AreEqual(1, dungeon.zones[3][0].neighbors.Count); //neighbor should be only the endNode
            Assert.AreEqual(1, dungeon.zones[3][1].neighbors.Count); //neighbor should be only the endNode

            OgNode neighbor1 = dungeon.zones[3][0].neighbors[0]; //neighbor should be only the endNode
            OgNode neighbor2 = dungeon.zones[3][1].neighbors[0]; //neighbor should be only the endNode

            Assert.AreSame(neighbor1, neighbor2); //they should be the same neighbor endNode
            Assert.AreSame(neighbor1, dungeon.endNode); //The check

            Assert.AreEqual(2, dungeon.endNode.neighbors.Count); //endNode should have 2 neighbors
            Assert.IsTrue(dungeon.endNode.neighbors.Contains(dungeon.zones[3][0]) && dungeon.endNode.neighbors.Contains(dungeon.zones[3][1]));
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
            List<OgNode> path = dungeon.shortestPath(dungeon.bridges[1], dungeon.endNode); //Path from bridge 1 to endNode

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

        [TestMethod]
        public void DungeonMonsterBalansTest()
        {
            int difficulty = 100;
            int totalMonsters = 10 * difficulty;
            int constant = (difficulty + 2) * (difficulty + 1);

            Dungeon dungeon = new Dungeon(difficulty);

            int total = 0;
            for(int i = 1; i <= difficulty + 1; i++)
            {
                int monstersInZone = 0;

                foreach(Node node in dungeon.zones[i])
                    monstersInZone = monstersInZone + node.monsterAmount;

                double monstersInThisZone = (2 * i * totalMonsters) / (float)constant;
                int monstersToAdd = (int)monstersInThisZone;

                Assert.IsTrue((monstersToAdd == monstersInZone || (monstersToAdd + 1) == monstersInZone));

                total = total + monstersInZone;
            }

            Assert.AreEqual(totalMonsters, total);
        }

        [TestMethod]
        public void DungeonHPTest()
        {
            Dungeon dungeon = new Dungeon(100, 300); // 300 = current player HP + potions in his bag

            // Sum of all potions
            int totalHP = 0;
            foreach (List<OgNode> zone in dungeon.zones)
            {
                foreach (OgNode node in zone)
                {
                    foreach (Item item in node.items)
                    {
                        if (item.GetType() == typeof(Potion))
                            totalHP = totalHP + 20;
                    }
                }
            }

            totalHP = totalHP + 300;

            // total monster HP
            int totalHPmonsters = 0;
            foreach (List<OgNode> zone in dungeon.zones)
            {
                foreach (OgNode node in zone)
                {
                    foreach (Pack pack in node.monsters)
                    {
                        foreach (Monster monster in pack.pack)
                            totalHPmonsters = totalHPmonsters + monster.HP;
                    }
                }
            }

            Assert.IsTrue(totalHP <= totalHPmonsters);
        }

    }
}
