using System;
using System.Collections.Generic;
using System.Text;

namespace Items
{
    public class Armor: Item
    {
        int armor;
        Armor(string _name, int _armor, int _value, int _durability)
        {
            name = _name;
            armor = _armor;
            value = _value;
            durability = _durability;
        }
    }
}
