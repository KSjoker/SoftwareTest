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
            foreach (Node node in dungeon.nodes)
            {
                Console.WriteLine("CurrentNode: " + node.Name());
                foreach (Node neighbor in node.neighbors)
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
            foreach (Node node in shortestPath)
            {
                Console.WriteLine("Node: " + node.Name());
            }

            Console.WriteLine("");

            // Display level of any node
            OgNode randomNode = dungeon.bridges[1];
            int level = dungeon.level(randomNode);
            Console.WriteLine("level of node: " + randomNode.Name() + " = " + level.ToString());

            Console.WriteLine("");

            // Testing destroying of bridge
            dungeon.destroy(dungeon.bridges[1]);
            foreach (Node node in dungeon.nodes)
            {
                Console.WriteLine("CurrentNode: " + node.Name());
                foreach (Node neighbor in node.neighbors)
                {
                    Console.WriteLine("Neighbors are: " + neighbor.Name());
                }
                Console.WriteLine("");
            }

            Console.ReadLine();
        }
    }
}
