using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeoneering_Server
{
    class Dungeon
    {
        private Random r;
        public Dungeon()
        {
            r = new Random();

            Quest();
        }

        private void Quest()
        {
            int tier = r.Next(1, 4);
            if(tier == 1)
            {
                Console.WriteLine("You are fighting a goblin");
            }
            if (tier == 2)
            {
                Console.WriteLine("You are fighting a orc");
            }
            if (tier == 3)
            {
                Console.WriteLine("You are fighting a dragon");
            }
        }
    }
}
