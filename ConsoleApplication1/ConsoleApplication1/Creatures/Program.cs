using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

            Console.WriteLine("Press any key to start new Game");
            Console.ReadLine();

            //Game copy = DeepClone<Game>(game);

            while (gaming)
            {
                OgNode currentNode = game.player.currentNode;
                Player player = game.player;
                Dungeon dungeon = game.dungeon;

                //MONSTERS MOVE TO PLAYER IN SAME ZONE
                Console.WriteLine("MONSTER POSITION INFO------------------------------------------");
                if (currentNode.zone != -1) //We are in a zone which has a level of >= 1
                    foreach (OgNode node in dungeon.zones[currentNode.zone])
                    {
                        Pack[] packs = node.monsters.ToArray();
                        for (int i = 0; i < packs.Length; i++)
                            packs[i].MoveTowardsPlayer(currentNode);
                    }
                else
                    Console.WriteLine("No Monsters moved towards you....");

                // Player information
                Console.WriteLine(" ");
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
                    Console.WriteLine("Current Node type = Bridge \n");
                else if (currentNode.Name() == "Begin")
                    Console.WriteLine("Current Node type = Start \n");
                else if (currentNode.Name() == "End")
                    Console.WriteLine("Current Node type = End \n");
                else
                    Console.WriteLine("Current Node type = Plain Node \n");

                // Available edges at Current Node
                Console.WriteLine("EDGE INFORMATION-----------------------------------------------");
                Console.WriteLine("Available edges at Current Node:");
                foreach (OgNode neighbor in currentNode.neighbors)
                    Console.Write(neighbor.Name() + "   ");

                // Monster packs currently in Current Node
                Console.WriteLine(" ");
                Console.WriteLine(" ");
                Console.WriteLine("MONSTER INFORMATION--------------------------------------------");
                Console.WriteLine("Monster packs currently in Current Node: ");
                if (currentNode.monsters.Count == 0)
                    Console.Write("No Monsters \n");
                for (int i = 1; i <= currentNode.monsters.Count(); i++)
                {
                    Console.WriteLine("Monster pack " + i.ToString() + " :");
                    Console.WriteLine("Amount = " + currentNode.monsters[i - 1].Count.ToString());
                    Console.WriteLine("Total HP = " + currentNode.monsters[i - 1].totalHealth.ToString());
                }

                Console.WriteLine(" ");
                Console.WriteLine("GAME--------------------------------------------------------");
                // THE GAME

                // Check for monsters
                currentNode.Contested(); //Check if current Node is contested or not
                bool continueCombat = true;
                bool bridgeDestroy = false;
                int DestroyedBridge = -1;
                while (currentNode.contested && continueCombat)
                {
                    bool input1 = true;
                    Console.WriteLine("Combat is about to Begin...");
                    while (input1)
                    {
                        switch (player.getCommand())
                        {
                            case 2:
                                player.UseItem(new Potion(20));
                                break;
                            case 3:
                                player.UseItem(new TimeCrystal());
                                if (currentNode.nodeLevel > 0 && ((Node)currentNode).crystalUsed)
                                {
                                    DestroyedBridge = currentNode.zone;
                                    dungeon.BridgeDestroy(currentNode);
                                    bridgeDestroy = true;
                                    input1 = false;
                                }
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
                    if (bridgeDestroy)
                        break;

                    // Player starts Combat
                    ((Node)currentNode).doCombat(currentNode.monsters[0], player);
                    input1 = true;

                    while (input1)
                    {
                        switch (player.getCommand())
                        {
                            case 1:
                                continueCombat = false;
                                input1 = false;
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
                if (!bridgeDestroy)
                {
                    Console.WriteLine("Pick a destination: write ID of edge, if you write something else you will stay in the Current Node");
                    string answer = Console.ReadLine();
                    foreach (OgNode neighbor in currentNode.neighbors)
                    {
                        if (answer == neighbor.Name())
                        {
                            player.Move(neighbor);
                            if (answer == "End")
                                game.nextDungeon();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Bridge has been destroyed, you will be moved to safe neighboring node");
                    player.Move(dungeon.zones[DestroyedBridge + 1][0]);
                }
            }
        }

        //Deep Copy object
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

    }
}
