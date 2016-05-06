using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Pack
    {
        /* TODO:-Implement Move
        */
        public List<Monster> pack;
        public Pack(int HP, int AP, int lowerBound, int upperBound = 0)
        {
            //Generates a new pack of monsters. If the fourth parameter is left out, the pack size will equal that of lowerBound. 
            //Else, the pack size will be a random number between lowerBound and upperBound
            {
                Random rand = new Random();
                int amount;
                if (upperBound == 0)
                    amount = lowerBound;
                else
                    amount = rand.Next(lowerBound, upperBound);

                for (int i = 0; i < amount; i++)
                    pack.Add(new Monster(HP, AP));
            }
        }
        void Move()
        {
            // Wait for nodes
        }

        void Attack(Creature x)
        {
            int damage;
            if (pack.Count > 0)
            {
                damage = pack[0].AR * pack.Count;
                x.HP = x.HP - damage;
            }
        }


    }
}
