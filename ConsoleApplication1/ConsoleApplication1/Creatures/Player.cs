using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApplication1
{
    class Player : Creature
    {
        /* TODO: -Implement Move functionality
                 -Implement Timecrystal functionality
                 -Implement Retreat functionality
                 -Change Attack to work with Timecrystal
        */


        int MaxHP;
        int potions;
        int healAmount = 10; // amount of healing from potions
        int timeCrystals;
        string input;
        enum items {POTION, TIMECRYSTAL };
        Node currentNode, lastNode;
        
        //Node location;
        public Player(int hp, int maxhp, int ar)
        {
            MaxHP = maxhp;
            hitPoints = hp;
            attackRating = ar;
        }
        void getCommand()
        {
            switch (input)
            {
                case "move": Move(); break;

                case "potion": UseItem(items.POTION); break;

                case "time": UseItem(items.TIMECRYSTAL); break;

                case " retreat": Retreat(); break;

                case "attack": break;

            }

        }
        void Attack(Pack p)
        {
            if (true)//no timecrystal in use
            {
                p.pack[0].HP = p.pack[0].HP - AR;

                if (p.pack[0].HP < 1)
                    p.pack.RemoveAt(0);
            }
            else //Timecrystal in use
            {
                for (int i = 0; i < p.pack.Count; i++)
                {
                    p.pack[i].HP = p.pack[i].HP - AR;
                }
                for (int i = 0; i < p.pack.Count; i++)
                {
                    if (p.pack[0].HP < 1)
                        p.pack.RemoveAt(0);
                }
            }
        }

        void Move()
        {
            //Do stuff
        }


        void UseItem(items i)
        {
            if(i == items.POTION)
            {
                if (potions > 0)
                {
                    potions--;
                    HP = Math.Min(HP + healAmount, MaxHP);
                }
            }
            else
            {
                //use timecrystal
            }

        }

        void Retreat()
        {
            //GTFO
        }
    }
}
