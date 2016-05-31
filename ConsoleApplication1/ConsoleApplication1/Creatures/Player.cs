using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApplication1
{
    [Serializable]
    public class Player : Creature
    {
        /* TODO: -Implement Timecrystal functionality
                 -Change Attack to work with Timecrystal
        */

        public int killpoint=0;
        public int maxHP;
        public List<Potion> potions = new List<Potion>();
        public List<TimeCrystal> crystals = new List<TimeCrystal>();
        protected string input;
        public OgNode currentNode, lastNode;
        
        //Node location;
        public Player(int hp, int maxhp, int ar,  OgNode begin)
        {
            maxHP = maxhp;
            hitPoints = hp;
            attackRating = ar;
            currentNode = begin;
            currentNode.bplayer = true;
            CollectItems(currentNode);
        }
        public int getCommand()
        {
            Console.WriteLine("What do you want to do? 1 = Move, 2 = Use Potion, 3 = use Time Crystal, 4 = Retreat, 5 = Continue Combat");
            while (true)
            {
                int answer = int.Parse(Console.ReadLine());
                if (answer <= 5 && answer > 0)
                    return answer;
                else
                    Console.WriteLine("Please write a number between 0 and 6");
            }
        }
        public void Attack(Pack p)
        {
            if (!((Node)currentNode).crystalUsed)//no timecrystal in use
            {
                Console.WriteLine("Monster HP was " + p.pack[0].HP.ToString());
                p.pack[0].HP = p.pack[0].HP - AR;
                Console.WriteLine("Monster HP is now " + p.pack[0].HP.ToString());
                if (p.pack[0].HP < 1)
                {
                    killpoint++;
                    p.pack.RemoveAt(0);
                    Console.WriteLine("You have killed the monster");
                }
            }
            else //Timecrystal in use
            {
                Console.WriteLine("Time Crystal is in use, every monster is attacked");
                for (int i = 0; i < p.Count; i++)
                    p.pack[i].HP = p.pack[i].HP - AR;

                for (int i = 0; i < p.pack.Count;)
                {
                    if (p.pack[0].HP < 1)
                    {
                        killpoint++;
                        p.pack.RemoveAt(0);
                    }
                    else
                        i++;
                }

                ((Node)currentNode).crystalUsed = false;
            }
        }

        public void Move(OgNode target)
        {
            currentNode.bplayer = false;
            lastNode = currentNode;
            currentNode = target;
            currentNode.bplayer = true;
            CollectItems(currentNode);
        }

        public void UseItem(Item i)
        {
            if(i.GetType() == typeof(Potion))
            {
                if (potions.Count > 0)
                {
                    HP = Math.Min(HP + potions[0].HP, maxHP);
                    Console.WriteLine("Potion used");
                    potions.RemoveAt(0);
                }
                else
                    Console.WriteLine("You don't have any Potions");
            }
            else 
            {
                if (currentNode.Name() != "Begin" && currentNode.Name() != "End" && crystals.Count > 0)
                {
                    ((Node)currentNode).crystalUsed = true;
                    Console.WriteLine("Time Crystal used");
                    crystals.RemoveAt(0);
                }
                else
                    Console.WriteLine("You don't have any crystals");
            }

        }

        public void Retreat()
        {
            OgNode temp = currentNode;
            currentNode = lastNode;
            lastNode = temp;

        }

        public void CollectItems(OgNode node)
        {
            foreach(Item item in node.items)
            {
                if (item.GetType() == typeof(Potion))
                    potions.Add((Potion)item);
                else
                    crystals.Add((TimeCrystal)item);
            }

            node.items = new List<Item>(); 
        }
    }
}
