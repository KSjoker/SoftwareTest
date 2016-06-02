using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalRNGSwitch
{
    public static class GlobalRNG
    {
        static GlobalRNG() { RNGSwitch = false; } // Switch --> True = RNG, False = !RNG

        public static bool RNGSwitch { get; private set; }
    }
}

namespace SaveAndReplay
{
    public static class SaveGame
    {
        static SaveGame() {
            GameSession = new List<string>(); //List of gameStates that represent a game-session
            Replay = false;
        } 

        public static List<string> GameSession { get; set; }
        public static bool Replay { get; set; }
    }
}
