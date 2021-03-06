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
            prefix = _element;
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
                case "lightning":
                    prefixDmg = Ligtning();
                    break;
                case "inferno":
                    prefixDmg = Inferno();
                    break;
                case "water":
                    prefixDmg = Water();
                    break;
                case "bat":
                    prefixDmg = Bat();
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

        public int Inferno()
        {
            return (12);
        }
        public int Water()
        {
            Random rnd = new Random();
            return (rnd.Next(1,50));
        }
        public int Bat()
        {
            return (16);
        }
    }
}
