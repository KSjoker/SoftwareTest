using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Dungeon
    {
        List<OgNode> nodes, bridges;

        public Dungeon(int difficulty)
        {
        }
    }

    abstract class OgNode
    {
        public int maxMonsters, nodeLevel;
        public List<Item> items;
        protected string name;
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

    class EndNode : Node
    {
        public EndNode(int nLevel)
            : base(nLevel, "end")
        {
            maxMonsters = nodeLevel = 0;
        }

        public override string Name()
        {
            return name;
        }
    }

    class Node : OgNode
    {
        float m;
        bool contested;
        public bool crystalUsed;
        public List<Pack> monsters;
        public List<Node> neighbors;

        public Node(int nLevel,string nName)
        {
            nodeLevel = nLevel;
            m = 0.9f;
            maxMonsters = (int)m*(nodeLevel+1);
            name = nName;
        }

        public override string Name()
        {
            return name;
        }
    }
}
