using System;
using System.Collections.Generic;
using System.Text;

namespace Items
{
    public class Weapon : Item
    {
        public int damage;
        public string owner;
        public string prefix;
        public Weapon(string _name, int _damage, string _element, string _owner)
        {
            prefix = "fire";
            Name = $"{prefix} sword";
            damage = _damage;
            owner = _owner;
        }

        public int attack()
        {
            var totalAttack = 0;
            int prefixDmg = 0;

            switch (prefix)
            {
                case "fire":
                prefixDmg = fire();
                    break;
            }

            totalAttack = damage + prefixDmg;

            return totalAttack;

        }

        public int fire()
        {
            return (6);
        }
        public int Ligtning()
        {
            Random rnd = new Random();
            return (rnd.Next(3, 12));
        }
    }
}
