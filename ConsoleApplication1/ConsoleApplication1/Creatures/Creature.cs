using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    abstract class Creature
    {
       protected int hitPoints;
       protected int attackRating;

        public int HP
        {
            get { return hitPoints; }
            set { hitPoints = value; }
        }

        public int AR
        {
            get { return attackRating; }
        }


    }
}
