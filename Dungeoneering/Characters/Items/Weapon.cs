using System;
using System.Collections.Generic;
using System.Text;

namespace Items
{
    public class Weapon : Item
    {
        public int damage;
        public string prefix;
        public Weapon(string _name, int _damage, int _value, int _durability)
        {
            prefix = "fire";
            Name = $"{prefix} sword";
            damage = _damage;
            value = _value;
            durability = _durability;
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
