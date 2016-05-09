using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Dungeon
    {
        public List<OgNode> nodes = new List<OgNode>(); //List of every node in the graph
        public List<OgNode>[] zones; //List of zones
        public OgNode[] bridges; // Array of bridges
        public BeginNode beginNode = new BeginNode();
        public EndNode endNode = new EndNode();
        int ID;

        public Dungeon(int difficulty)
        {
            //Every Node (which is not a begin, bridge or end node) will be named as follows:
            // node-"zone level"-"ID"
            //Every Bridge will be named as:
            // bridge-"zone level"
            // The Begin and End nodes are named as "begin" and "end"

            int ID = 0; 

            // We want our bridges in one array for quick look-up
            bridges = new OgNode[difficulty + 1]; // We don't use bridges[0]. Memory waste yes, but it's more easy to work with

            // We want our zones in seperately
            zones = new List<OgNode>[difficulty + 2]; //+2 because of the end-zone (= highest bridge level + 1)
            for (int i = 0; i < difficulty + 2; i++)
                zones[i] = new List<OgNode>(); //create a new list on zonelevel index

            // This is our current node where we will start creating the graph
            OgNode currentStart = beginNode;

            // First add zones
            for (int i = 1; i <= difficulty; i++)
            {
                Add_DaimondZone(currentStart, i, false);
                currentStart = bridges[i];
            }

            // Then add last zone with an endNode
            Add_DaimondZone(currentStart, difficulty + 1, true);
        }

        public void Add_DaimondZone(OgNode begin, int zoneLevel, bool end)
        {
            // Create new nodes and a bridge
            Node node1 = new Node(0, "node-" + zoneLevel.ToString() + "-" + UniqueID().ToString());
            Node node2 = new Node(0, "node-" + zoneLevel.ToString() + "-" + UniqueID().ToString());
            OgNode bridge;
            if (!end)
                bridge = new Node(zoneLevel, "bridge-" + zoneLevel.ToString());
            else
                bridge = endNode;

            // Create edges between new nodes, bridge and begin node
            node1.neighbors.Add(bridge);
            node1.neighbors.Add(begin);

            node2.neighbors.Add(bridge);
            node2.neighbors.Add(begin);

            bridge.neighbors.Add(node1);
            bridge.neighbors.Add(node2);

            begin.neighbors.Add(node1);
            begin.neighbors.Add(node2);

            // Add these new nodes to the list (to keep track), and add the bridge to array of bridges.
            // The bridge is not added here if end is false, why? Because the bridge will be added as "begin" in the second iteration.
            nodes.Add(begin);
            nodes.Add(node1);
            nodes.Add(node2);
            if (end)
                nodes.Add(bridge);
            else
                bridges[zoneLevel] = bridge;

            // Add nodes to own zone
            zones[zoneLevel].Add(node1);
            zones[zoneLevel].Add(node2);
            if(!end)
                zones[zoneLevel].Add(bridge);
        }

        public List<OgNode> shortestPath(OgNode start, OgNode end)
        {
            List<List<OgNode>> open = new List<List<OgNode>>(); //Paths we need to process
            List<OgNode> closed = new List<OgNode>(); //Nodes that we already processed

            List<OgNode> firstPath = new List<OgNode>();
            firstPath.Add(start);

            open.Add(firstPath); //Our first path ---> open = [[start]]

            while(open.Count > 0) // While there are still paths that can be extended with a not-yet-seen neighboring node
            {
                // BFS SEARCH
                List<List<OgNode>> copy = open.ToList(); // Our paths that all have the same length (depth)
                open.Clear(); // We clear the open list so we can add new paths that are 1 node longer

                foreach(List<OgNode> path in copy) //Foreach path that we have
                {
                    OgNode lastNodeInPath = path[path.Count - 1]; //Take the last node of the path
                    foreach(OgNode neighbor in lastNodeInPath.neighbors) //For every neighbor of that last node
                    {
                        if (!closed.Contains(neighbor)) //If we haven't seen that neighbor node earlier
                        {
                            // We will create a new path by copying the current path and adding the neighbor node
                            List<OgNode> newPath = path.ToList();
                            newPath.Add(neighbor);
                            
                            // If this node is our end destination we can return the path
                            if(neighbor == end)
                                return newPath;
                            else //Else, we will add it to the open list so that it will be processed in the following iteration
                                open.Add(newPath);
                        }
                    }

                    // Add lastNodeinPath to closed because we have processed it
                    closed.Add(lastNodeInPath);
                }
            }

            // This method will return null if (start == end), or if (start to end is not possible/doesn't exist)
            return null;
        }

        public int level(OgNode node)
        {
            string name = node.Name();
            if (name[1] == 'r') //If the second letter is 'r', we know it's a bridge 
            {
                string[] splitName = name.Split('-');
                int level;
                if (Int32.TryParse(splitName[1], out level))
                    return level;
            }

            // else we now it's node of level 0
            return 0;
        }

        // This method is kinda messy and not efficient at all
        // However is does the job; it removes all certain node references from every list where this reference is present
        // Unfortunately, references are copied when added to a list. So we need to check multiple lists
        public void BridgeDestroy(OgNode bridge){

            foreach (OgNode neighbor in bridge.neighbors)
                neighbor.neighbors.Remove(bridge);
            

            for (int i = 1; i <= level(bridge); i++)
            {
                // Remove every node of the current zone in nodes and zones
                // And delete every node of the current zone in every nodes's neighbors
                foreach (OgNode removedNode in zones[i])
                    nodes.Remove(removedNode);
                
                // Delete the bridge of this zone as well
                bridges[i] = null;
                zones[i] = new List<OgNode>();
            }
            nodes.Remove(beginNode);
            beginNode = null;
        }

        int UniqueID() {
            ID++;
            return ID - 1; 
        }
    }

    public abstract class OgNode
    {
        public int maxMonsters, nodeLevel;
        public List<Item> items;
        public List<Pack> monsters;
        public List<OgNode> neighbors;
        protected string name;
        public abstract string Name();
    }

    public class BeginNode : OgNode 
    {
        public BeginNode()
        {
            maxMonsters = nodeLevel = 0;
            name = "begin";
            neighbors = new List<OgNode>();
        }

        public override string Name()
        {
            return name;
        }
    }

    public class EndNode : OgNode
    {
        public EndNode()
        {
            name = "end";
            maxMonsters = nodeLevel = 0;
            neighbors = new List<OgNode>();
        }

        public override string Name()
        {
            return name;
        }
    }

    public class Node : OgNode
    {
        float m;
        public bool contested,player,crystalUsed;

        public Node(int nLevel,string nName)
        {
            nodeLevel = nLevel;
            m = 1.0f;
            maxMonsters = (int)m*(nodeLevel+1);
            name = nName;
            neighbors = new List<OgNode>();
        }

        public override string Name()
        {
            return name;
        }

        public void Contested()
        {
            if (player && monsters.Count > 0)
                contested = true;
            else contested = false;
        }
    }

    public class NillNode : OgNode
    {
        public override string Name()
        {
            return "Nill";
        }
    }
}