using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Game
    {
        public Dungeon dungeon;
        public Player player;
        public List<Pack> monsters = new List<Pack>();
        public List<Item> items = new List<Item>(); 

        public Game()
        {
            nextDungeon();
            player = new Player(100, 100, 3, dungeon.beginNode);
            monsterListUpdate();
            itemListUpdate();
        }

        //Creates a dungeon
        public void nextDungeon()
        {
            if (dungeon == null)
                dungeon = new Dungeon(1);
            else
                dungeon = new Dungeon(dungeon.bridges.Length, player.HP + (player.potions.Count * 20));
        }

        // Updates monster list to contain all packs of monsters that currently roam the dungeon
        public void monsterListUpdate()
        {
            foreach (List<OgNode> zone in dungeon.zones)
            {
                foreach (OgNode node in zone)
                {
                    foreach (Pack pack in node.monsters)
                    {
                        monsters.Add(pack);
                    }
                }
            }
        }

        // Updates item list to contain all item that currently are scattered throughout the dungeon 
        public void itemListUpdate()
        {
            foreach (List<OgNode> zone in dungeon.zones)
            {
                foreach (OgNode node in zone)
                {
                    foreach (Item item in node.items)
                    {
                        items.Add(item);
                    }
                }
            }
        }
    }
}
