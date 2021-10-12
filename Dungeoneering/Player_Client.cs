using Dungeon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeoneering_Server
{
    class Player_Client
    {
        public Player character;
        private string IpAdress;

        public Player_Client(string IP,string name,int str,int dex)
        {
            this.IpAdress = IP;
            character = new Player(name,str,dex);
        }



    }
}
