using Dungeon;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Dungeoneering_Server
{
    public class Player_Client
    {
        public Player character;
        public string IpAdress;
        public TcpClient client;
        public string input;
        public bool Dungeoneering;

        public Player_Client(TcpClient client,string IP,string name, string password, string salt, int str,int dex,int lvl)
        {
            this.client = client;
            this.IpAdress = IP;
            character = new Player(client, name, password, salt, str,dex,lvl);
        }



    }
}
