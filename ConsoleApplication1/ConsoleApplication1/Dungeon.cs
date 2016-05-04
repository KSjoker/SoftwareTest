using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Dungeon
    {

    }

    abstract class OgNode
    {
        protected int maxMonsters, nodeLevel,healthPotions, timeCrystals;
        protected string name;
        public abstract string toString();
        public abstract string Name();
    }

    class BeginNode : Node 
    {
        public BeginNode(int nLevel)
            : base(nLevel,"begin")
        {
            maxMonsters = nodeLevel = 0;
        }
    }

    class EndNode : OgNode
    {
        public EndNode()
        {
            maxMonsters = 0;
            nodeLevel = 0;
            healthPotions = 0;
            timeCrystals = 0;
            name = "end";
        }

        public override string Name()
        {
            return name;
        }

        public override string toString()
        {
            return "endnode";
        }
    }

    class Node:OgNode
    {
        float m;
        bool contested;
        Edge north, south, east, west;

        public Node(int nLevel,string nName)
        {
            nodeLevel = nLevel;
            m = 0.9f;
            maxMonsters = (int)m*(nodeLevel+1);
            name = nName;
        }

        public override string toString() 
        {
            String eNorth,eEast,eSouth,eWest;

            if (north == null)
                eNorth = "null";
            else eNorth = North.ToString();

            if (east == null)
                eEast = "null";
            else eEast = East.ToString();

            if (south == null)
                eSouth = "null";
            else eSouth = South.ToString();

            if (west == null)
                eWest = "null";
            else eWest = West.ToString();

            return "north: "+ eNorth + " ,east: "+ eEast +" ,south: "+ eSouth+ " ,west: "+ eWest ;
        }

        public override string Name()
        {
            return name;
        }

        public Edge North
        {
            get { return north; }
            set { north = value;}
        }

        public Edge East
        {
            get { return east; }
            set { east = value; }
        }

        public Edge South
        {
            get { return south; }
            set { south = value; }
        }

        public Edge West
        {
            get { return west; }
            set { west = value; }
        }
 
    }

    class Edge
    {
        OgNode node1;
        OgNode node2;
        bool isEmpty = false;

        public Edge(OgNode n1, OgNode n2)
        {
            node1 = n1;
            node2 = n2;
        }

        public bool IsEmpty
        {
            get {     if (node2 == null || node1 == null)
                        isEmpty = true;
                      return isEmpty;}

            set { isEmpty = value; }
        }

        public override string ToString()
        {
            string n1,n2;
            if (node1 != null)
                n1 = node1.Name();
            else n1 = "null";

            if (node2 != null)
                n2 = node2.Name();
            else n2 = "null";

            return n1 + " to " + n2;
        }
    }
}
