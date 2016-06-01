using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    [Serializable]
    public class Pack
    {

        public List<Monster> pack;
        Node currentNode;
        Random rand = new Random();
        public int zone;
        public Pack(Node node, int HP, int AP, int lowerBound, int upperBound = 0)
        {
            //Generates a new pack of monsters. If the fourth parameter is left out, the pack size will equal that of lowerBound. 
            //Else, the pack size will be a random number between lowerBound and upperBound
            pack = new List<Monster>();

            currentNode = node;
            int amount;
            if (upperBound == 0)
                amount = lowerBound;
            else
                amount = rand.Next(lowerBound, upperBound);
            for (int i = 0; i < amount; i++)
                pack.Add(new Monster(HP, AP));

            zone = node.zone;
        }

        public void Move()
        {
            //select a target node
            int options = currentNode.neighbors.Count - 1;
            OgNode target = currentNode.neighbors[rand.Next(options)];

            //Can't go into target if it's a begin or end Node, so stop trying to  movve
            if (target.GetType() == typeof(EndNode) || target.GetType() == typeof(BeginNode))
                return;

            //find number of monsters already present in node
            int monstersInNode = 0;
            for(int i = 0; i< target.monsters.Count; i++)
                monstersInNode += target.monsters[i].Count;
            

            //If there's space for the pack, move to target
            if(monstersInNode + this.Count < target.maxMonsters && target.zone == currentNode.zone)
            {
                currentNode.monsters.Remove(this);
                target.monsters.Add(this);
                currentNode = (Node)target;
                Console.WriteLine("The Monster pack moved to " + target.Name());
            }
        }

        public void MoveTowardsPlayer(OgNode playerNode)
        {
            //select a target node
            List<OgNode> shortestPath = Dungeon.shortestPath(currentNode, playerNode, true);
            if (shortestPath == null)
            {
                Console.WriteLine("A Monster pack wanted to move, but failed");
                return;
            }

            OgNode target = shortestPath[1];

            //find number of monsters already present in node
            int monstersInNode = 0;
            for (int i = 0; i < target.monsters.Count; i++)
                monstersInNode += target.monsters[i].Count;

            //If there's space for the pack, move to target
            if (monstersInNode + this.Count < target.maxMonsters)
            {
                currentNode.monsters.Remove(this);
                target.monsters.Add(this);
                currentNode = (Node)target;
                Console.WriteLine("A Monster pack moved to " + target.Name());
            }
        }

        public void Attack(Creature x)
        {
            int damage;
            if (pack.Count > 0)
            {
                damage = pack[0].AR * pack.Count;
                x.HP = x.HP - damage;
            }
        }

        public int Count
        {
            get { return pack.Count; }
        }

        public int totalHealth
        {
            get
            {
                int total = 0;

                for (int i = 0; i < pack.Count; i++)
                    total += pack[i].HP;

                return total;
            }
        }

    }
}
