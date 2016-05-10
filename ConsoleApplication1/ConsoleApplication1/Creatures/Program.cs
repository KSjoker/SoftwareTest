using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static Dungeon dungeon;

        static void Main(string[] args)
        {
            dungeon = new Dungeon(1);

            // Displaying every node in the dungeon
            foreach (OgNode node in dungeon.nodes)
            {
                Console.WriteLine("CurrentNode: " + node.Name());
                foreach (OgNode neighbor in node.neighbors)
                {
                    Console.WriteLine("Neighbors are: " + neighbor.Name());
                }
                Console.WriteLine("");
            }

            // Displaying the shortpath from an existing node to another existing node
            OgNode from = dungeon.beginNode;
            OgNode to = dungeon.endNode;
            List<OgNode> shortestPath = dungeon.shortestPath(from, to);
            Console.WriteLine("Shortest Path from " + from.Name() + " to " + to.Name() + " is");
            foreach (OgNode node in shortestPath)
            {
                Console.WriteLine("Node: " + node.Name());
            }

            Console.WriteLine("");

            // Display level of any node
            OgNode randomNode = dungeon.bridges[1];
            int level = dungeon.level(randomNode);
            Console.WriteLine("level of node: " + randomNode.Name() + " = " + level.ToString());

            Console.WriteLine("");

            //// Testing destroying of bridge
            //dungeon.BridgeDestroy(dungeon.bridges[1]);
            //foreach (OgNode node in dungeon.nodes)
            //{
            //    Console.WriteLine("CurrentNode: " + node.Name());
            //    foreach (OgNode neighbor in node.neighbors)
            //    {
            //        Console.WriteLine("Neighbors are: " + neighbor.Name());
            //    }
            //    Console.WriteLine("");
            //}

            // Testing monsterCount
            foreach (List<OgNode> zone in dungeon.zones)
            {
                int total = 0;
                foreach (OgNode node in zone)
                {
                    foreach (Pack pack in node.monsters)
                    {
                        Console.WriteLine(node.Name() + " Numbers of Monsters = " + pack.Count.ToString());
                        total = total + pack.Count;
                    }
                }
                Console.WriteLine("total = " + total.ToString());
            }

            Console.WriteLine(" ");

            // Testing items
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
            Console.WriteLine("total HP player + potions = " + totalHP.ToString());

            // Monster HP
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

            Console.WriteLine("total HP monsters = " + totalHPmonsters.ToString());

            //foreach (List<OgNode> list in dungeon.zones)
            //{
            //    foreach (OgNode node in list)
            //    {
            //        Console.WriteLine("CurrentNode: " + node.Name());
            //        foreach (OgNode neighbor in node.neighbors)
            //        {
            //            Console.WriteLine("Neighbors are: " + neighbor.Name());
            //        }
            //        Console.WriteLine("");
            //    }
            //}

            Console.ReadLine();
        }
    }
}
