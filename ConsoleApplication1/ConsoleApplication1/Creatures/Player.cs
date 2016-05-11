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

        protected int KillPoint=0;
        protected int MaxHP;
        public List<Potion> potions = new List<Potion>();
        protected List<TimeCrystal> crystals = new List<TimeCrystal>();
        protected string input;
        protected OgNode currentNode, lastNode;
        
        //Node location;
        public Player(int hp, int maxhp, int ar,  OgNode begin)
        {
            MaxHP = maxhp;
            hitPoints = hp;
            attackRating = ar;
            currentNode = begin;
        }
        public virtual void getCommand()
        {
            switch (input)
            {
                case "move": Move(new Node(0,"Dummy")); break;

                case "potion": UseItem(potions[0]); break;

                case "time": UseItem(crystals[0]); break;

                case " retreat": Retreat(); break;

                case "attack": break;

            }

        }
        public void Attack(Pack p)
        {
            if (!((Node)currentNode).crystalUsed)//no timecrystal in use
            {
                p.pack[0].HP = p.pack[0].HP - AR;

                if (p.pack[0].HP < 1)
                {
                    KillPoint++;
                    p.pack.RemoveAt(0);
                }
            }
            else //Timecrystal in use
            {
                for (int i = 0; i < p.pack.Count; i++)
                    p.pack[i].HP = p.pack[i].HP - AR;

                for (int i = 0; i < p.pack.Count; i++)
                {
                    if (p.pack[0].HP < 1)
                    {
                        KillPoint++;
                        p.pack.RemoveAt(0);
                    }
                }
            }
        }

        protected void Move(Node target)
        {
            lastNode = currentNode;
            currentNode = target;
        }


        protected void UseItem(Item i)
        {
            if(i.GetType() == typeof(Potion))
            {
                if (potions.Count > 0)
                {
                    potions.RemoveAt(0);
                    HP = Math.Min(HP + potions[0].HP, MaxHP);
                }
            }
            else
            {
                if(currentNode.Name() != "begin" && currentNode.Name() != "end")
                ((Node)currentNode).crystalUsed = true;
            }

        }

        protected void Retreat()
        {
            currentNode = lastNode;
        }
    }


    class dummyPlayer : Player
    {
        public dummyPlayer(int hp, int  maxhp,int  ar, ref OgNode n): base(hp, maxhp,  ar,n)
        {
            MaxHP = maxhp;
            hitPoints = hp;
            attackRating = ar;
            currentNode = n;
        }
        //Insert custom commands here
        public override void getCommand()
        {
            Move(new Node(0,"Dummy"));
            UseItem(potions[0]);
        }
    }
}
