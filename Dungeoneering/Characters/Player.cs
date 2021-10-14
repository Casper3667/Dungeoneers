using System;
using System.Collections.Generic;
using System.Text;
using Items;
using _Defines;

namespace Dungeon
{
    public class Player : Character
    {
        public int str;
        public int dex;
        public List<Item> inventory;
        Weapon sword;
        public Player(string _name, int _str, int _dex,int lvl)
        {
            Level = lvl;
            name = _name;
            str = _str;
            dex = _dex;
            sword = new Weapon("Basic sword", 2, 2, 100);
            inventory = new List<Item>();
            inventory.Add(sword);

            this.damage = (str * 1);
            this.hp = (50 * Level);
        }
        public override void Attack(Character target)
        {
            int damage = str + sword.damage; // placeholder until weapons are implemented
            target.TakeDamage(damage);
        }

        public void TakeItem(Item target)
        {
            if (inventory.Contains(target))
                return;
            inventory.Add(target);
        }

        public string GetInventory()
        {
            return _Helper.NiceList<Item>(inventory);
        }
    }
}
