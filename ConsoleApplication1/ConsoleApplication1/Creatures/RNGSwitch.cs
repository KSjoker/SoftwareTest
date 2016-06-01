using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalRNGSwitch
{
    public static class GlobalRNG
    {
        static GlobalRNG() { RNGSwitch = true; } // Switch --> True = RNG, False = !RNG

        public static bool RNGSwitch { get; private set; }
    }
}
