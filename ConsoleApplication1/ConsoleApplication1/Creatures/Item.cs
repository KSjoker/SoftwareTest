using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public abstract class Item
    {
        public int HP;
        public bool time;
    }

    public class Potion : Item
    {
        public Potion(int hp)
        {
            this.HP = hp;
            this.time = false;
        }
    }

    public class TimeCrystal : Item
    {
        public TimeCrystal()
        {
            this.HP = 0;
            this.time = true;
        }
    }
}
