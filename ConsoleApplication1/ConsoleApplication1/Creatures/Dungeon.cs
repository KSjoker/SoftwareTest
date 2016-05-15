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
        public List<OgNode>[] zones; //List of zones in the graph
        public OgNode[] bridges; // Array of bridges
        public BeginNode beginNode = new BeginNode();
        public EndNode endNode = new EndNode();
        int ID; // ID for every node

        Random random = new Random();

        public Dungeon(int difficulty, int currentHP = 0)
        {
            //Every Node (which is not a begin, bridge or end node) will be named as follows:
            // node-"zone level"-"ID"
            //Every Bridge will be named as:
            // bridge-"zone level"
            // The Begin and End nodes are named as "begin" and "end"

            int ID = 0; 

            // We want our bridges in one array for quick look-up
            bridges = new OgNode[difficulty + 1]; // We don't use bridges[0]. Memory waste yes, but it's more easy to work with

            // We want our zones organized as well
            zones = new List<OgNode>[difficulty + 2]; //+2 because of the end-zone (= highest bridge level + 1)
            for (int i = 0; i < difficulty + 2; i++)
                zones[i] = new List<OgNode>(); //create a new list on zonelevel index

            // This is our current node where we will start creating the graph
            OgNode currentStart = beginNode;

            // First add zones
            for (int i = 1; i <= difficulty; i++)
            {
                Add_DiamondZone(currentStart, i, false);
                currentStart = bridges[i];
            }

            // Then add last zone with an endNode
            Add_DiamondZone(currentStart, difficulty + 1, true);

            // We can now add monsters to these nodes
            int totalMonsters = 10 * difficulty;
            int constant = (difficulty + 2) * (difficulty + 1);
            double rest = 0;

            for (int i = 1; i < difficulty + 2; i++)
                rest = Add_MonstersToZone(i, totalMonsters, constant, rest);

            // After the monsters we can add items to the dungeon
            int totalHPmonsters = 0;
            foreach (List<OgNode> zone in zones)
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

            int totalHPplayer = currentHP;
            int HPtoAdd = totalHPmonsters - totalHPplayer;

            //Adding items
            for (int i = 1; i < difficulty + 2; i++)
                HPtoAdd = HPtoAdd - Add_Items(i, HPtoAdd);

        }

        int Add_Items(int zone, int HPtoAdd)
        {
            int HPAdded = 0;

            // 25% chance foreach node to contain a tymecrystal
            // 50% chance foreach node to contain a potion
            foreach (OgNode node in zones[zone])
            {
                if (random.Next(1, 5) == 1) //Add crystal with a 25% chance of success
                    node.items.Add(new TimeCrystal());

                int health = 20;
                if ((HPtoAdd - health) >= 0) //If we may add a potion 
                {
                    if (random.Next(1, 3) == 1) //Add potion with a 50% chance of success
                    {
                        node.items.Add(new Potion(health));
                        HPAdded = HPAdded + health;
                        HPtoAdd = HPtoAdd - health;
                    }
                }
            }

            return HPAdded;

        }

        // This method adds monsters to a zone. It returns a "rest" value which is used to round numbers correctly (for monsterBalans)
        double Add_MonstersToZone(int zone, int totalMonst, int constant, double rest)
        {
            //Formula
            double monstersInThisZone = (2 * zone * totalMonst) / (float)constant;
            int monstersToAdd = (int)monstersInThisZone;
            rest = rest + (monstersInThisZone - monstersToAdd);
            
            if (rest >= 0.99f && rest <= 1) { rest = 1; }
            if (rest >= 1.0f)
            {
                monstersToAdd++;
                rest--;
            }

            //Adding the monsters
            int currentlyAdded = 0;
            while (currentlyAdded != monstersToAdd)
            {
                currentlyAdded = currentlyAdded + addMonster(zone, monstersToAdd, currentlyAdded);
            }

            return rest;
        }

        // This method returns how many monsters are added to the zone
        int addMonster(int zone, int toAdd, int added)
        {
            int totalAdded = 0;
            toAdd = toAdd - added;

            foreach (Node node in zones[zone])
            {
                if (toAdd == 0) //If we can't add any more monsters
                    break;

                Pack newPack = new Pack(node, 10, 10, 1, toAdd + 1); //Create new Pack with a random number of monsters

                if (newPack.Count + node.monsterAmount <= node.maxMonsters) //Check node constraint (doesn't really matter actually, this "if" is always true)
                    node.AddMonsters(newPack);

                int monstersAdded = newPack.pack.Count; //Number of monsters added
                toAdd = toAdd - monstersAdded;
                totalAdded = totalAdded + monstersAdded;
            }

            return totalAdded;
        }

        void Add_DiamondZone(OgNode begin, int zoneLevel, bool end)
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

        public int level(OgNode node) {
            return node.nodeLevel;
        }

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
        public bool bplayer;
    }

    public class BeginNode : OgNode 
    {
        public BeginNode()
        {
            maxMonsters = nodeLevel = 0;
            name = "Begin";
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
            name = "End";
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
        public bool contested, crystalUsed;
        public int monsterAmount;

        public Node(int nLevel,string nName)
        {
            nodeLevel = nLevel;
            m = 20.0f;
            maxMonsters = (int)m*(nodeLevel+1);
            name = nName;
            neighbors = new List<OgNode>();
            monsters = new List<Pack>();
            items = new List<Item>();
            monsterAmount = 0;
        }

        public override string Name()
        {
            return name;
        }

        public void doCombat(Pack p, Player player)
        {
            bool end = false;
            while (player.HP > 0 && p.Count > 0 && !end)
                end = doCombatRound(p,player);

            if (p.Count == 0)
                monsters.Remove(p);
        }

        public bool doCombatRound(Pack p, Player player)
        {
            //player.getCommand();
            player.Attack(p);
            p.Attack(player);

            if (p.Count == 0)
                monsters.Remove(p);

            //player.getCommand();
            if (p.totalHealth < player.HP)
            {
                if (p.totalHealth > 0)
                {
                    p.Move();
                    return true;
                }
            }

            return false;
        }
        public void Contested()
        {
            if (bplayer && monsters.Count > 0)
                contested = true;
            else contested = false;
        }

        public void AddMonsters(Pack newPack)
        {
            monsters.Add(newPack);
            monsterAmount = monsterAmount + newPack.Count;
        }
    }

    public class NillNode : OgNode
    {
		public NillNode()
		{
			name = "Nill";
		}
		
        public override string Name()
        {
            return name;
        }
    }
}