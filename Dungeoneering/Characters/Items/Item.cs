using System;
using System.Collections.Generic;
using System.Text;

namespace Items
{
    public abstract class Item
    {
        protected string name;
        public int value;
        public int durability;

        public string Name { get => name; set => name = value; }
    }
}
