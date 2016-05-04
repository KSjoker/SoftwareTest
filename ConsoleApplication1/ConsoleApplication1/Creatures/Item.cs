using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public abstract class Item
    {
        
    }

    public class Potion : Item
    {
        public int HP;
        public Potion(int hp)
        {
            HP = hp;

        }
    }

    public class TimeCrystal : Item
    {
        public TimeCrystal()
        {
        }
    }
}
