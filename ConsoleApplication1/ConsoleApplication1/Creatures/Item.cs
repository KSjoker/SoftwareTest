using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    [Serializable]
    public abstract class Item
    {
        
    }

    [Serializable]
    public class Potion : Item
    {
        public int HP;
        public Potion(int hp)
        {
            HP = hp;

        }
    }

    [Serializable]
    public class TimeCrystal : Item
    {
        public TimeCrystal()
        {
        }
    }
}
