using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public static class Tests
    {
        static public bool CapacityTest(List<Game> testcase)
        {
            Console.WriteLine("Starting capacity test");

            for (int i = 0; i < testcase.Count; i++)
            {
                Game game = testcase[i];
                foreach (List<OgNode> zone in game.dungeon.zones)
                    foreach(Node n in zone)
                        if (n.monsterAmount > n.maxMonsters)
                        {
                            Console.WriteLine("Error:");
                            Console.WriteLine("Node over capacity at iteration [0]", i);
                            return false;
                        }
            }

            Console.WriteLine("No problem with capacity found");
            return true;
        }

        static public bool ZoneTest(List<Game> testcase)
        {
            Console.WriteLine("Starting zone test");

            for (int i = 0; i < testcase.Count - 1; i++)
            {
                Game game = testcase[i];
                Player player = game.player;
                Game game2 = testcase[i + 1];
                Player player2 = game2.player;
                foreach (Pack p in game.monsters)
                {
                    if (p.zone == player.currentNode.zone)
                    {
                        Pack p2 = game2.monsters.Find(x => x.ID == p.ID);
                        List<OgNode> dist = Dungeon.shortestPath(p.currentNode, player.currentNode, true);
                        List<OgNode> dist2 = Dungeon.shortestPath(p2.currentNode, player2.currentNode, true);
                        if (dist2 != null && dist != null)
                        {
                            if (dist2.Count - dist.Count > 0)
                            {
                                Console.WriteLine("Error:");
                                Console.WriteLine("Pack moved wrong at iteration [0]", i);
                                return false;
                            }
                        }
                    }
                }
            }

            Console.WriteLine("No issues with monster movement");
            return true;
        }

        static public bool MonsterZoneTest(List<Game> games)
        {
            Console.WriteLine("Starting monster zone test");

            bool test = true;
            foreach (Game game in games)
                foreach (Pack monster in game.monsters)
                    test &= monster.zone == monster.currentNode.zone;

            if (test)
                Console.WriteLine("Monster zones satisfies constraint");
            else
                Console.WriteLine("Monster zones does not satisfy constraint");
            return test;
        }
    }
}
