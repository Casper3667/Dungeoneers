using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Net.Sockets;
using Items;

namespace Dungeoneering_Server.Repository
{
    interface IDatabaseMapper
    {
        List<Player_Client> ReadAllClientsFromMapper(SQLiteDataReader reader, TcpClient client);

        List<Weapon> ReadAllWeaponsFromMapper(SQLiteDataReader reader);
    }
}
