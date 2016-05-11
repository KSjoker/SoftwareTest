using System;
using ConsoleApplication1;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class CreatureTest
    {
        [TestMethod]
        public void TestPlayer()
        {
            int hp = 100;
            int maxhp = 100;
            int ar = 10;
            //create player
            Player player = new Player(hp, maxhp, ar,new Node(2, "test"));
            Assert.AreEqual(hp, player.HP);
            Assert.AreEqual(maxhp,player.maxHP);
            Assert.AreEqual(ar,player.AR);

            player.HP = player.maxHP - 40;

            Assert.AreEqual(player.maxHP - 40,player.HP);
            Assert.AreEqual(maxhp, player.maxHP);

            player.potions.Add(new Potion(30));

            Assert.IsTrue(player.potions.Count == 1);

            player.UseItem(player.potions[0]);
            Assert.IsTrue(player.HP == maxhp-40+30);
            Assert.IsTrue(player.potions.Count == 0);

            player.crystals.Add(new TimeCrystal());
            Assert.IsTrue(player.crystals.Count == 1);

            player.UseItem(player.crystals[0]);
            Assert.IsTrue(((Node)player.currentNode).crystalUsed);
            Assert.IsTrue(player.crystals.Count == 0);


        }

        [TestMethod]
        public void TestPackCreation()
        {
            int hp = 10;
            int ar = 10;
            int count = 6;
            int upper = 10;
            Pack pack = new Pack(new Node(2, "test"), hp, ar, count);

            Assert.AreEqual(count * hp, pack.totalHealth);
            Assert.AreEqual(ar, pack.pack[0].AR);
            Assert.AreEqual(count, pack.Count);

            pack = new Pack(new Node(2, "test"), hp, ar, count, upper);

            Assert.IsTrue(pack.Count > 5 && pack.Count < 11);
        }

        [TestMethod]
        public void TestAttack()
        {
            Node testNode = new Node(2, "test");
            Player player = new Player(100, 100,10, testNode);
            int hp = 10;
            int higherHP = 15;
            int ar = 10;
            int count = 6;
            Pack pack = new Pack(testNode, hp, ar, count);

            pack.Attack(player);
            //Check the pack deals correct damage
            Assert.AreEqual(100 - ar * count, player.HP);

            player.Attack(pack);
            //Check if a creature died
            Assert.IsTrue(pack.Count == 5);
            Assert.IsTrue(player.killpoint == 1);

            player.crystals.Add(new TimeCrystal());
            player.UseItem(player.crystals[0]);
            player.Attack(pack);
            //All monsters should die from this attack
            Assert.AreEqual(0, pack.Count);
            Assert.IsTrue(player.killpoint == 6);
            pack = new Pack(testNode, higherHP, ar, count);
            testNode.crystalUsed = false;
            player.Attack(pack);

            //monster should not be dead
            Assert.AreEqual(5,pack.pack[0].HP);

            player.crystals.Add(new TimeCrystal());
            player.UseItem(player.crystals[0]);
            player.Attack(pack);

            //Only first monster should be dead
            Assert.AreEqual(5, pack.Count);
        }
        [TestMethod]
        public void MoveTest()
        {
            //Dungeon with 3 nodes, one way movement only
            Node node1 = new Node(0, "1");
            Node node2 = new Node(0, "2");
            Node node3 = new Node(0, "3");
            EndNode end = new EndNode();

            node1.neighbors.Add(node2);
            node2.neighbors.Add(node3);
            node3.neighbors.Add(end);

            node1.monsters.Add(new Pack(node1, 10, 10, 10));

            //Monsters should move to node 2
            node1.monsters[0].Move();

            Assert.IsTrue(node1.monsters.Count == 0);
            Assert.IsTrue(node2.monsters.Count == 1);

            node3.monsters.Add((new Pack(node1, 10, 10, 15)));

            //Monsters try to move to node3, can't becuase it would be over capacity
            //This means there will be a pack in node3 and node2
            node2.monsters[0].Move();

            Assert.IsTrue(node1.monsters.Count == 0);
            Assert.IsTrue(node2.monsters.Count == 1);
            Assert.IsTrue(node3.monsters.Count == 1);

            //Monsters try to move, but can't becuase they're moving into endNode
            node3.monsters[0].Move();
            Assert.IsTrue(node3.monsters.Count == 1);
            node2.monsters.RemoveAt(0);

            Player player = new Player(10, 10, 10, node1);

            player.Move(node2);
            Assert.AreEqual(node1, player.lastNode);
            Assert.AreEqual(node2, player.currentNode);

            player.Retreat();
            Assert.AreEqual(node2, player.lastNode);
            Assert.AreEqual(node1, player.currentNode);
        }

    }
}
