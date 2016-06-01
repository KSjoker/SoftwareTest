using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    static class CapacityTest
    {

        static public void Test(List<Game> g)
        {
            List<Game> testcase = g;
            for (int i = 0; i < testcase.Count;i++)
            {
                Game game = testcase[i];
                foreach(Node n in game.dungeon.nodes)
                {
                    if (n.monsterAmount > n.maxMonsters)
                    {
                        Console.WriteLine("false");
                        Console.WriteLine("Node over capacity at [0]", i);
                    }
                }
            }
            Console.WriteLine("true");
        }
    }
}
