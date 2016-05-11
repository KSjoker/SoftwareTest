using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApplication1
{
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
        }
        public virtual void getCommand()
        {
           
        }
        public void Attack(Pack p)
        {
            if (!((Node)currentNode).crystalUsed)//no timecrystal in use
            {
                p.pack[0].HP = p.pack[0].HP - AR;

                if (p.pack[0].HP < 1)
                {
                    killpoint++;
                    p.pack.RemoveAt(0);
                }
            }
            else //Timecrystal in use
            {
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
            }
        }

        public void Move(Node target)
        {
            lastNode = currentNode;
            currentNode = target;
        }


        public void UseItem(Item i)
        {
            if(i.GetType() == typeof(Potion))
            {
                if (potions.Count > 0)
                {
                    HP = Math.Min(HP + potions[0].HP, maxHP);
                    potions.RemoveAt(0);
                }
            }
            else if(i.GetType() == typeof(TimeCrystal))
            {
                if(currentNode.Name() != "begin" && currentNode.Name() != "end")
                ((Node)currentNode).crystalUsed = true;
                crystals.RemoveAt(0);
            }

        }

        public void Retreat()
        {
            OgNode temp = currentNode;
            currentNode = lastNode;
            lastNode = temp;

        }
    }


    public class dummyPlayer : Player
    {
        public dummyPlayer(int hp, int  maxhp,int  ar, ref OgNode n): base(hp, maxhp,  ar,n)
        {
            maxHP = maxhp;
            hitPoints = hp;
            attackRating = ar;
            currentNode = n;
        }
        //Insert custom commands here
        public override void getCommand()
        {

        }
    }
}
