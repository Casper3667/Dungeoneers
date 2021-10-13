using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace Dungeoneering_Server.Repository
{
    class DatabaseMapper : IDatabaseMapper
    {
        public List<Player_Client> ReadAllClientsFromMapper(SQLiteDataReader reader)
        {
            List<Player_Client> results = new List<Player_Client>();
            while (reader.Read())
            {
                string name = reader.GetString(0);
                int level = reader.GetInt32(1);
                int damage = reader.GetInt32(2);
                int health = reader.GetInt32(3);
                
                //results.Add(new Player_Client()

            }
            return results;
        }
    }
}
