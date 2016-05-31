using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    [Serializable]
    public class Monster : Creature
    {
        public Monster(int hp, int ar)
        {
            hitPoints = hp;
            attackRating = ar;
        }
    }
}
