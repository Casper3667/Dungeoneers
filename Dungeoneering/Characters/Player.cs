using System;
using System.Collections.Generic;
using System.Text;
using Items;

namespace Dungeon
{
    public class Player : Character
    {
        int str;
        int dex;
        Weapon Sword;
        public Player(string _name, int _str, int _dex)
        {
            name = _name;
            str = _str;
            dex = _dex;
            Sword = new Weapon("Sword", 4, 5, 100);
        }
        public override void Attack(Character target)
        {
            int damage = str + Sword.damage; // placeholder until weapons are implemented
            target.TakeDamage(damage);
        }
    }
}
