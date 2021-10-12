using System;
using System.Collections.Generic;
using System.Text;
using Items;
using _Defines;

namespace Dungeon
{
    public class Player : Character
    {
        int str;
        int dex;
        public List<Item> inventory;
        Weapon Sword;
        public Player(string _name, int _str, int _dex)
        {
            name = _name;
            str = _str;
            dex = _dex;
            Sword = new Weapon("Basic sword", 2, 2, 100);
            inventory = new List<Item>();
            inventory.Add(Sword);
        }
        public override void Attack(Character target)
        {
            int damage = str + Sword.damage; // placeholder until weapons are implemented
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
