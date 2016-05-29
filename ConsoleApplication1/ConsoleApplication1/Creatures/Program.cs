﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating new Game...");
            //Creating new game
            Game game = new Game();
            bool gaming = true;

            Console.WriteLine("Start new Game");
            Console.ReadLine();

            while(gaming)
            {
                OgNode currentNode = game.player.currentNode;
                Player player = game.player;
                Dungeon dungeon = game.dungeon;

                // Player information
                Console.WriteLine("PLAYER INFORMATION---------------------------------------------");
                Console.WriteLine("Player HP = " + player.HP.ToString());
                Console.WriteLine("Player AR = " + player.AR.ToString());
                Console.WriteLine("Player KP = " + player.killpoint.ToString());
                Console.WriteLine("Potions = " + player.potions.Count());
                Console.WriteLine("Time Crystals = " + player.crystals.Count() + "\n");

                // Node information
                Console.WriteLine("NODE INFORMATION-----------------------------------------------");
                Console.WriteLine("Current Node ID = " + currentNode.Name());
                if (currentNode.nodeLevel > 0)
                    Console.WriteLine("Current Node type = Bridge");
                else if (currentNode.Name() == "begin")
                    Console.WriteLine("Current Node type = Start");
                else if (currentNode.Name() == "end")
                    Console.WriteLine("Current Node type = End");
                else
                    Console.WriteLine("Current Node type = Plain Node \n");

                // Available edges at Current Node
                Console.WriteLine("EDGE INFORMATION-----------------------------------------------");
                Console.WriteLine("Available edges at Current Node:");
                foreach(OgNode neighbor in currentNode.neighbors)
                    Console.Write(neighbor.Name() + "   ");

                // Monster packs currently in Current Node
                Console.WriteLine(" ");
                Console.WriteLine(" ");
                Console.WriteLine("MONSTER INFORMATION--------------------------------------------");
                Console.WriteLine("Monster packs currently in Current Node: ");
                if (currentNode.monsters.Count == 0)
                    Console.Write("No Monsters");
                for(int i = 1; i <= currentNode.monsters.Count(); i++)
                {
                    Console.WriteLine("Monster pack " + i.ToString() + " :");
                    Console.WriteLine("Amount = " + currentNode.monsters[i - 1].Count.ToString());
                    Console.WriteLine("Total HP = " + currentNode.monsters[i - 1].totalHealth.ToString());
                }

                Console.WriteLine("\n GAME--------------------------------------------------------");
                // THE GAME

                // Check for monsters
                currentNode.Contested(); //Check if current Node is contested or not
                bool continueCombat = true;
                while (currentNode.contested && continueCombat)
                {
                    Console.WriteLine("CurrentNode is Contested!");
                    bool input1 = true;
                    while (input1)
                    {
                        switch (player.getCommand())
                        {
                            case 2:
                                player.UseItem(new Potion(20));
                                Console.WriteLine("Potion used");
                                break;
                            case 3:
                                player.UseItem(new TimeCrystal());
                                Console.WriteLine("Crystal used");
                                break;
                            case 5:
                                input1 = false;
                                break;
                            default:
                                Console.WriteLine("Not permitted at this stage: Choose 2, 3 or 5");
                                break;
                        }
                    }

                    //Time Crystal is used on a bridge
                    if (((Node)currentNode).crystalUsed && currentNode.nodeLevel > 0)
                        break;

                    // Player chooses which pack he/she wants to attack
                    Console.WriteLine("Start Combat");
                    ((Node)currentNode).doCombat(currentNode.monsters[0], player);
                    Console.WriteLine("Combat is over, what do you want to do?");
                    input1 = true;

                    while (input1)
                    {
                        switch (player.getCommand())
                        {
                            case 1:
                                continueCombat = false;
                                break;
                            case 5:
                                input1 = false;
                                break;
                            default:
                                Console.WriteLine("Not permitted at this stage: Choose 1 or 5");
                                break;
                        }
                    }

                    currentNode.Contested(); //Check if current Node is contested or not
                }

                // Player can now choose where he/she wants to go next
                Console.WriteLine("Pick a destination: write ID of edge, if you fuck up you stay");
                string answer = Console.ReadLine();
                foreach(OgNode neighbor in currentNode.neighbors)
                {
                    if (answer == neighbor.Name())
                        player.Move(neighbor);
                }

                Console.ReadLine();
            }
        }
    }
}
