using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using SaveAndReplay;

namespace ConsoleApplication1
{
    class Program
    {
        static List<Game> gameStates;
        static string path;
        static void Main(string[] args)
        {
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1 = Play, save and test a game session");
            Console.WriteLine("2 = Replay and test a saved game session");
            string answer = Console.ReadLine();
            path = @"";

            // If we want to replay a game session we need to load it first
            if (answer == "2")
            {
                SaveAndReplay.SaveGame.Replay = true;
                string[] lines = System.IO.File.ReadAllLines(path);
                foreach (string line in lines)
                    SaveAndReplay.SaveGame.GameSession.Add(line);
            }

            PlayGame();

            //Test session
            Tests();

            Console.ReadLine();
        }

        //Deep Copy object
        static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        static void PlayGame()
        {
            Console.WriteLine("Creating new Game...");
            //Creating new game
            Game game = new Game();
            bool gaming = true;

            Console.WriteLine("Game Starts");
            Console.WriteLine(" ");

            // List of game states
            gameStates = new List<Game>();

            // ADDING START GAME STATE
            gameStates.Add(DeepClone<Game>(game));

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
                        {
                            if (!packs[i].moved) //If we haven't moved the pack yet
                                packs[i].MoveTowardsPlayer(currentNode);
                        }
                    }
                else
                    Console.WriteLine("No Monsters moved towards you....");

                // Set moved boolean to false;
                foreach (Pack pack in game.monsters)
                    pack.moved = false;

                // ADDING GAME STATE
                gameStates.Add(DeepClone<Game>(game));

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

                // ADDING GAME STATE
                gameStates.Add(DeepClone<Game>(game));

                bool continueCombat = true;
                bool bridgeDestroy = false;
                bool gameOver = false;
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

                        //Update monster list
                        game.monsterListUpdate();

                        // ADDING GAME STATE
                        gameStates.Add(DeepClone<Game>(game));
                    }

                    //Time Crystal is used on a bridge
                    if (bridgeDestroy)
                        break;

                    // Player starts Combat
                    if (!((Node)currentNode).doCombat(currentNode.monsters[0], player))
                    {
                        //Update monster list
                        game.monsterListUpdate();

                        // ADDING GAME STATE
                        gameStates.Add(DeepClone<Game>(game));

                        gameOver = true;
                        break;
                    }

                    //Update monster list
                    game.monsterListUpdate();

                    // ADDING GAME STATE
                    gameStates.Add(DeepClone<Game>(game));

                    // After combat
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

                    // ADDING GAME STATE
                    gameStates.Add(DeepClone<Game>(game));
                }

                if (!gameOver)
                {
                    // Player can now choose where he/she wants to go next
                    if (!bridgeDestroy)
                    {
                        Console.WriteLine("Pick a destination: write ID of edge, if you write something else you will stay in the Current Node");
                        string answer;
                        if (SaveAndReplay.SaveGame.Replay) //Replaying the game
                        {
                            answer = SaveAndReplay.SaveGame.GameSession[0]; // Take answer
                            SaveAndReplay.SaveGame.GameSession.RemoveAt(0); // Remove answer
                        }
                        else //Playing actual session
                        {
                            answer = Console.ReadLine();
                            SaveAndReplay.SaveGame.GameSession.Add(answer);
                        }

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
                else
                    game = new Game();

                //Update monster/item list
                game.monsterListUpdate();
                game.itemListUpdate();

                // ADDING GAME STATE
                gameStates.Add(DeepClone<Game>(game));

                Console.WriteLine("Do you want to continue playing? Yes or No");
                while (gaming)
                {
                    string answer;
                    if (SaveAndReplay.SaveGame.Replay) // Replaying the game
                    {
                        answer = SaveAndReplay.SaveGame.GameSession[0]; // Take answer
                        SaveAndReplay.SaveGame.GameSession.RemoveAt(0); // Remove answer
                    }
                    else //Playing actual session
                    {
                        answer = Console.ReadLine();
                        SaveAndReplay.SaveGame.GameSession.Add(answer);
                    }
                    
                    if (answer == "Yes")
                        break;
                    else if (answer == "No")
                        gaming = false;
                    else
                        Console.WriteLine("Please write Yes or No");
                }
            }

            // Saving session if not replaying
            if (!SaveAndReplay.SaveGame.Replay)
            {
                System.IO.File.WriteAllLines(path,
                    SaveAndReplay.SaveGame.GameSession);
            }
        }

        static void Tests()
        {
            Console.WriteLine("STARTING ALL TESTS-------------------------------------------------");
            Console.WriteLine(" ");

            bool[] tests = new bool[4];
            bool test = true;

            //Testing play session
            tests[0] = ConsoleApplication1.Tests.CapacityTest(gameStates);
            tests[1] = ConsoleApplication1.Tests.MonsterZoneTest(gameStates);
            tests[2] = ConsoleApplication1.Tests.KillPointTest(gameStates);
            tests[3] = ConsoleApplication1.Tests.ItemPickUpTest(gameStates);
            test &= (tests[0] && tests[1] && tests[2] && tests[3]);

            if (test)
                Console.WriteLine("All test requirements achieved");
            else
                Console.WriteLine("Not All test requirements achieved");

            //Checking coverage
            float total = 0;
            foreach (bool covered in tests)
                if (covered)
                    total++;

            Console.WriteLine("Achieved = " + ((total / 4) * 100).ToString() + "%");

            if (total != 4)
                for (int i = 0; i < 4; i++)
                    if (!tests[i])
                        Console.WriteLine("Test " + i.ToString() + " was not covered");

            Console.WriteLine("  ");
        }

    }
}
