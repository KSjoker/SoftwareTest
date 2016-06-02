using ConsoleApplication1;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class NodeTest
    {
        [TestMethod]
        public void NodeTest_Name()
        {
            Node node = new Node(0, "Hello", 1);
            string expectedName = "Hello";

            string testName = node.Name();
            Assert.AreEqual(testName,expectedName);

            BeginNode begin = new BeginNode();
            string expectedName2 = "Begin";

            string testName2 = begin.Name();
            Assert.AreEqual(testName2, expectedName2);
            
            EndNode end = new EndNode();
            string expectedName3 = "End";

            string testName3 = end.Name();
            Assert.AreEqual(testName3, expectedName3);
            
            NillNode nill = new NillNode();
            string expectedName4 = "Nill";

            string testName4 = nill.Name();
            Assert.AreEqual(testName4, expectedName4);
        }

        [TestMethod]
        public void NodeTest_AddMonsters()
        {
            Node node = new Node(0, "hello", 1);
            Pack dummy = new Pack(1, node, 10, 10, 2, 5);
            node.AddMonsters(dummy);
            bool monstercountbt0 = node.monsters.Count > 0;
            bool monsteramountbt0 = node.monsterAmount > 0;
            bool monstercount = node.monsters.Count == 1;
            bool monsteramount = node.monsterAmount >= 2 && node.monsterAmount < 5;

            bool expected = true;
            bool testAmounts = monsteramountbt0 && monstercountbt0;
            bool testAmounts2 = monsteramount && monstercount;

            Assert.AreEqual(testAmounts2, expected);
            Assert.AreEqual(testAmounts, expected);

            Pack dummy2 = new Pack(2,node, 10, 10, 4, 10);
            node.AddMonsters(dummy2);
            monstercount = node.monsters.Count == 2;
            monsteramount = node.monsterAmount >= 6 && node.monsterAmount < 15;

            bool testAmounts3 = monsteramount && monstercount;
            Assert.AreEqual(testAmounts3, expected);

            Pack dummy3 = new Pack(3, node, 10, 10, 20);
            node.AddMonsters(dummy3);
            monstercount = node.monsters.Count == 3;
            monsteramount = node.monsterAmount >= 26 && node.monsterAmount < 35;

            bool testAmounts4 = monsteramount && monstercount;
            Assert.AreEqual(testAmounts4, expected);
        
        }

        [TestMethod]
        public void NodeTest_Contested()
        {
            Node node = new Node(0, "hello", 1);
            node.bplayer = true;
            node.Contested();

            Node node2 = new Node(0, "hello2", 1);
            node2.bplayer = false;
            node2.Contested();

            Node node3 = new Node(0, "hello3", 1);
            node3.bplayer = true;
            Pack dummy = new Pack(1, node3, 10, 10, 2, 5);
            node3.AddMonsters(dummy);
            node3.Contested();

            Node node4 = new Node(0, "hello4", 1);
            node4.bplayer = false;
            Pack dummy2 = new Pack(2, node4, 11, 11, 3, 4);
            node4.AddMonsters(dummy2);
            node4.Contested();

            bool contested = node.contested;
            bool contested2 = node2.contested;
            bool contested3 = node3.contested;
            bool contested4 = node4.contested;

            Assert.AreEqual(contested,false);
            Assert.AreEqual(contested2, false);
            Assert.AreEqual(contested3, true);
            Assert.AreEqual(contested4, false);
        }

        [TestMethod]
        public void NodeTest_DoCombat()
        {
            Node node = new Node(0, "Hello", 1);
            Node neighbor = new Node(0, "NEighbour", 1);
            node.neighbors.Add(neighbor);
            neighbor.neighbors.Add(node);
            Player player = new Player(100,100,4, node);
            Pack monsterp1 = new Pack(1, node,5,1,2,5);
            node.doCombat(monsterp1, player);

            bool check = node.monsters.Count == 0;
            Assert.AreEqual(check, true);
        }

        [TestMethod]
        public void NodeTest_DoCombatUntilDeath()
        {
            Node node = new Node(0, "Hello", 1);
            Node neighbor = new Node(0, "NEighbour", 1);
            node.neighbors.Add(neighbor);
            neighbor.neighbors.Add(node);
            Player player = new Player(100, 100, 4, node);
            Pack monsterp1 = new Pack(1, node, 5, 1, 2);
            node.AddMonsters(monsterp1);
            while (monsterp1.totalHealth != 0)
            {
                node.Contested();
                neighbor.Contested();

                if(node.contested)
                    node.doCombat(monsterp1, player);

                if(neighbor.contested)
                    neighbor.doCombat(monsterp1, player);

                node.Contested();
                neighbor.Contested();

                if (!node.contested && !neighbor.contested)
                {
                    OgNode neighboring = (player.currentNode.neighbors.ToArray())[0];
                    player.Move((Node)neighboring);
                }
            }

            bool check = node.monsters.Count == 0 && neighbor.monsters.Count == 0;
            Assert.AreEqual(check, true);
        }
    }
}
