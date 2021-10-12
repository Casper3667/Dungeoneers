using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeoneering_Server
{

    class Lobby
    {
        public List<Player_Client> Players = new List<Player_Client>();
        public string name;
        public Lobby(string name)
        {
            this.name = name;
        }

        public void join(Player_Client player)
        {
            Players.Add(player);
        }

    }
}
