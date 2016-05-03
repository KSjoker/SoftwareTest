using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace softtest
{
    class Dungeon
    {

    }

    abstract class OgNode
    {
        protected int maxMonsters, nodeLevel,healthPotions, timeCrystals;
    }

    class BeginNode : OgNode 
    {
        Edge nextEdge; 
        public BeginNode(Node nNext)
        {
            nextEdge = new Edge(this,nNext);
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
        }
    }

    class Node:OgNode
    {
        float m;
        bool contested;
        Edge north, south, east, west;

        public Node(int nLevel,Node nNorth,Node nSouth,Node nEast,Node nWest)
        {
            nodeLevel = nLevel;
            m = 0.9f;
            maxMonsters = (int)m*(nodeLevel+1);
            north = new Edge(this,nNorth);
            south = new Edge(this,nSouth);
            east  = new Edge(this,nEast);
            west  = new Edge(this,nWest);
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
            if (node2 == null)
                isEmpty = true;
        }
    }
}
