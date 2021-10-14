using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Net.Sockets;
using System.Text;

namespace Dungeoneering_Server.Repository
{
    class DatabaseMapper : IDatabaseMapper
    {
        public List<Player_Client> ReadAllClientsFromMapper(SQLiteDataReader reader, TcpClient client)
        {
            List<Player_Client> results = new List<Player_Client>();
            while (reader.Read())
            {
                string name = reader.GetString(0);
                int level = reader.GetInt32(1);
                int damage = reader.GetInt32(2);
                int health = reader.GetInt32(3);
                int dex = reader.GetInt32(4);

                results.Add(new Player_Client(client, client.Client.RemoteEndPoint.ToString(), name, damage, dex, level));

            }
            return results;
        }
    }
}
