using ConsoleApplication1;
using System.Collections.Generic;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void GameNextDungeonTest()
        {
            Game game = new Game();

            Assert.IsNotNull(game.dungeon);

            int oldBridge = game.dungeon.bridges.Length - 1;
            game.nextDungeon();
            int currentBridge = game.dungeon.bridges.Length - 1;

            Assert.AreEqual(currentBridge - 1, oldBridge);
        }

        [TestMethod]
        public void GameMonsterListTest()
        {
            Game game = new Game();
            int packCount = 0;
            foreach(List<OgNode> zone in game.dungeon.zones)
            {
                foreach (Node node in zone)
                {
                    foreach (Pack pack in node.monsters)
                    {
                        Assert.IsTrue(game.monsters.Contains(pack));
                        packCount++;
                    }
                }
            }

            Assert.AreEqual(packCount, game.monsters.Count);
        }

        [TestMethod]
        public void GameItemsListTest()
        {
            Game game = new Game();
            int itemCount = 0;
            foreach (List<OgNode> zone in game.dungeon.zones)
            {
                foreach (Node node in zone)
                {
                    foreach (Item item in node.items)
                    {
                        Assert.IsTrue(game.items.Contains(item));
                        itemCount++;
                    }
                }
            }

            Assert.AreEqual(itemCount, game.items.Count);
        }
    }
}
