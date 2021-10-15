using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Dungeoneering_Server.Repository
{
    interface IDatabaseRepository
    {
        void AddNewClient(string name, int level, int damage, int health, int dex);

        Player_Client FindAccount(string name, TcpClient client);

        void RemovePlayer(string name, TcpClient client);

        List<Player_Client> GetAllAccounts(TcpClient client);
    }
}
