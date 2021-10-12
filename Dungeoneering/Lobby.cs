using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeoneering_Server
{

    class Lobby
    {
        private List<Player_Client> Players = new List<Player_Client>();
        string name;
        public Lobby(string name)
        {
            this.name = name;
        }

    }
}
