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
            for (int i = 0; i < testcase.Count; i++)
            {
                Game game = testcase[i];
                foreach (Node n in game.dungeon.nodes)
                {
                    if (n.monsterAmount > n.maxMonsters)
                    {
                        Console.WriteLine("Error:");
                        Console.WriteLine("Node over capacity at iteration [0]", i);
                        return false;
                    }
                }
            }

            Console.WriteLine("No problem with capacity found");
            return true;
        }

        static public bool ZoneTest(List<Game> testcase)
        {
            for (int i = 0; i < testcase.Count - 1; i++)
            {
                Game game = testcase[i];
                Player player = game.player;
                Game game2 = testcase[i + 1];
                Player player2 = game2.player;
                foreach (Pack p in game.monsters)
                {
                    if (p.currentNode.zone == player.currentNode.zone)
                    {
                        Pack p2 = game2.monsters.Find(x => x.ID == p.ID);
                        int dist = Dungeon.shortestPath(p.currentNode, player.currentNode, true).Count;
                        int dist2 = Dungeon.shortestPath(p2.currentNode, player2.currentNode, true).Count;
                        if (dist2 - dist > 0)
                        {
                            Console.WriteLine("Error:");
                            Console.WriteLine("Pack moved wrong at iteration [0]", i);
                            return false;
                        }
                    }
                }
            }

            Console.WriteLine("No issues with monster movement");
            return true;
        }

        static public bool MonsterAmountTest(List<Game> states)
        {
            bool test = true;
            foreach (Game state in states)
            {
                foreach (Node node in state.dungeon.nodes)
                    test &= node.monsterAmount <= node.maxMonsters;
            }

            return test;
        }
    }
}
