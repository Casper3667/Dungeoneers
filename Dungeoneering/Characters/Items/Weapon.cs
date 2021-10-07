using System;
using System.Collections.Generic;
using System.Text;

namespace Items
{
    public class Weapon : Item
    {
        public int damage;
        public Weapon(string _name, int _damage, int _value, int _durability)
        {
            name = _name;
            damage = _damage;
            value = _value;
            durability = _durability;
        }
    }
}
