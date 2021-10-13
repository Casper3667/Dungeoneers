using System;
using System.Collections.Generic;
using System.Text;

namespace Items
{
    public abstract class Item
    {
        protected string name;
        protected int value;
        protected int durability;

        public string Name { get => name; set => name = value; }
    }
}
