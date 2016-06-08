using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public static class Tests
    {
        // Specification 1
        static public bool CapacityTest(List<Game> games)
        {
            Console.WriteLine("  ");
            Console.WriteLine("STARTING CAPACITY TEST----------------------------------------------");
            bool[] coverage = new bool[10];

            for (int i = 0; i < games.Count; i++)
            {
                Game nextState;
                if (i != games.Count - 1)
                    nextState = games[i + 1];
                else
                    nextState = null;

                Game game = games[i];
                foreach (List<OgNode> zone in game.dungeon.zones)
                    foreach (Node n in zone)
                    {
                        if (n.monsterAmount > n.maxMonsters)
                        {
                            Console.WriteLine("Error:");
                            Console.WriteLine("Node over capacity at iteration", i);
                            return false;
                        }

                        // Each Choice Coverage
                        if (n.nodeLevel == 0) //u’s level (0).
                            coverage[0] = true;
                        if (n.nodeLevel == 1) //u’s level (low).
                            coverage[1] = true;
                        if (n.nodeLevel > 1) //u’s level (high).
                            coverage[2] = true;

                        if (n.monsterAmount == 0) //u’s current occupancy(0)
                            coverage[3] = true;
                        if (n.monsterAmount >= 1 && n.monsterAmount < n.maxMonsters) //u’s current occupancy (between 1 and full)
                            coverage[4] = true;
                        if (n.monsterAmount == n.maxMonsters) //u’s current occupancy (already full).
                            coverage[5] = true;

                        if (i != games.Count - 1)
                        {
                            List<int> packIDsneighbors = new List<int>();
                            List<int> packIDscurrent = new List<int>();

                            foreach (OgNode neighbor in n.neighbors) //Find all packs in neighboring nodes
                            {
                                foreach (Pack pack in neighbor.monsters)
                                    packIDsneighbors.Add(pack.ID);
                            }

                            foreach (Pack pack in n.monsters) //Find all packs in current node
                                packIDscurrent.Add(pack.ID);


                            foreach (OgNode node in nextState.dungeon.nodes) //Find current node in next state
                            {
                                if (node.Name() == n.Name())
                                {
                                    // Check if a pack is moving to the current node
                                    foreach (Pack pack in node.monsters) //For every pack of current node in next state
                                        if (packIDsneighbors.Contains(pack.ID)) // Check if pack from neighboring node moved
                                        {
                                            coverage[6] = true;
                                            break;
                                        }

                                    // Check if a pack is leaving the current node
                                    foreach (OgNode neighbor in node.neighbors)
                                        foreach (Pack pack in neighbor.monsters)
                                            if (packIDscurrent.Contains(pack.ID))
                                            {
                                                coverage[7] = true;
                                                break;
                                            }
                                }
                            }
                        }
                    }
            }

            coverage[8] = true; // This is always true, we start at a begin node, no pack can leave or enter a begin node
            coverage[9] = true; // This is always true, we start at a begin node, no pack can leave or enter a begin node

            //Checking coverage
            int total = 0;
            foreach (bool covered in coverage)
                if (covered)
                    total++;

            Console.WriteLine("No problem with capacity found");
            Console.WriteLine("Each Choice Coverage = " + (total * 10).ToString() + "%");

            if (total != 10)
                for (int i = 0; i < 10; i++)
                    if (!coverage[i])
                        Console.WriteLine("Block " + i.ToString() + " was not covered");

            Console.WriteLine("  ");
            return true;
        }

        // Specification 2
        static public bool MonsterZoneTest(List<Game> games)
        {
            Console.WriteLine("STARTING MONSTER ZONE TEST------------------------------------------");
            bool[] coverage = new bool[7];

            bool test = true;

            for (int i = 0; i < games.Count; i++)
            {
                Game nextState;
                if (i != games.Count - 1)
                    nextState = games[i + 1];
                else
                    nextState = null;

                Game game = games[i];

                foreach (Pack monsterPack in game.monsters)
                {
                    test &= monsterPack.zone == monsterPack.currentNode.zone;

                    // Each Choice Coverage
                    if (monsterPack.currentNode.zone == 1)
                        coverage[0] = true; // the zone’s location (first).
                    if (monsterPack.currentNode.zone > game.dungeon.bridges.Length - (game.dungeon.bridges.Length - 1) && monsterPack.currentNode.zone < game.dungeon.bridges.Length)
                        coverage[1] = true; // the zone’s location (middle).
                    if (monsterPack.currentNode.zone == game.dungeon.bridges.Length)
                        coverage[2] = true; // the zone’s location (last).

                    if (i != games.Count - 1)
                    {
                        foreach (Pack pack in nextState.monsters)
                            if (pack.ID == monsterPack.ID)
                                if (pack.currentNode.Name() != monsterPack.currentNode.Name())
                                {
                                    if (monsterPack.currentNode == game.player.currentNode) //If monster and player are in the same node now
                                        coverage[3] = true; // Checking whether pack will flee from combat
                                    else
                                        coverage[4] = true;  // Checking whether pack will just move
                                }
                    }

                    if (monsterPack.currentNode.nodeLevel > 0)
                        coverage[5] = true; // the monster’s location (on a bridge)
                    else
                        coverage[6] = true; // the monster’s location (not on a bridge).
                }
            }

            if (test)
                Console.WriteLine("Monster zones satisfies constraint");
            else
                Console.WriteLine("Monster zones does not satisfy constraint");

            //Checking coverage
            float total = 0;
            foreach (bool covered in coverage)
                if (covered)
                    total++;

            Console.WriteLine("Each Choice Coverage = " + ((total / 7) * 100).ToString() + "%");

            if (total != 7)
                for (int i = 0; i < 7; i++)
                    if (!coverage[i])
                        Console.WriteLine("Block " + i.ToString() + " was not covered");

            Console.WriteLine("  ");
            return test;
        }

        // Specification 4
        static public bool KillPointTest(List<Game> games)
        {
            Console.WriteLine("STARTING KILLPOINT TEST---------------------------------------------");

            bool[] coverage = new bool[6];

            bool test = true;

            for (int i = 0; i < games.Count; i++)
            {
                Game nextState;
                if (i != games.Count - 1)
                    nextState = games[i + 1];
                else
                    nextState = null;

                Game game = games[i];

                bool currentUse, nextUse;

                if (i != games.Count - 1)
                {
                    // Checking if crystal is used by player in current State
                    if (game.player.currentNode.Name() == "Begin" || game.player.currentNode.Name() == "End")
                        currentUse = false;
                    else
                        currentUse = ((Node)game.player.currentNode).crystalUsed;

                    // Checking if crystal is used by player in next state
                    if (nextState.player.currentNode.Name() == "Begin" || nextState.player.currentNode.Name() == "End")
                        nextUse = false;
                    else
                        nextUse = ((Node)nextState.player.currentNode).crystalUsed;

                    // If the player has not used a crystal in the current and next state, 
                    // then KP + monsterAmount should be the same in both states
                    if (!currentUse && !nextUse)
                    {
                        int currentCount, nextCount;
                        currentCount = nextCount = 0;


                        // Count total monsters in current State
                        foreach (Pack pack in game.monsters)
                            currentCount += pack.pack.Count();

                        // Count total monsters in next State
                        foreach (Pack pack in nextState.monsters)
                            nextCount += pack.pack.Count();

                        test &= (game.player.killpoint + currentCount) == (nextState.player.killpoint + nextCount);
                    }

                    // Our coverage criteria
                    // We want a player(0 KP, >0 KP)
                    // We want a use and not a use of a crystal in the current and next State
                    if (game.player.killpoint == 0)
                        coverage[0] = true; // Player with KP = 0
                    else
                        coverage[1] = true; // Player with KP > 0

                    if (currentUse)
                        coverage[2] = true; // Use of crystal in current state
                    else
                        coverage[3] = true; // No Use of crystal in current state

                    if (nextUse)
                        coverage[4] = true; // Use of crystal in next state
                    else
                        coverage[5] = true; // No Use of crystal in next state
                }

            }

            if (test)
                Console.WriteLine("KillPoint balans satisfies constraint");
            else
                Console.WriteLine("KillPoint balans does not satisfy constraint");

            //Checking coverage
            float total = 0;
            foreach (bool covered in coverage)
                if (covered)
                    total++;

            Console.WriteLine("Each Choice Coverage = " + ((total / 6) * 100).ToString() + "%");

            if (total != 6)
                for (int i = 0; i < 6; i++)
                    if (!coverage[i])
                        Console.WriteLine("Block " + i.ToString() + " was not covered");

            Console.WriteLine("  ");
            return test;
        }

        // Specification 5, we can't tweak our monsters
        static public bool UniqueCombatTest(List<Game> games)
        {
            int uniqueCombats = 0;
            int difficulty = games[0].dungeon.bridges.Length;
            bool test = false;

            return test;
        }
    }
}
