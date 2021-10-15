using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Net.Sockets;

namespace Dungeoneering_Server.Repository
{
    class DatabaseRepository : IDatabaseRepository
    {
        private readonly IDatabaseProvider provider;
        private readonly IDatabaseMapper mapper;
        public DatabaseRepository(IDatabaseProvider provider, IDatabaseMapper mapper)
        {
            this.provider = provider;
            this.mapper = mapper;

            CreateDatabase();
        }
        
        private void CreateDatabase()
        {
            IDbConnection con = provider.CreateConnection();
            con.Open();

            //SQLiteCommand cmd = new SQLiteCommand("DROP TABLE clients", (SQLiteConnection)con);
            //cmd.ExecuteNonQuery();

            SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS clients(Name STRING PRIMARY KEY, Level INTEGER, Damage INTEGER, Health INTEGER, Dexterity INTEGER);", (SQLiteConnection)con);
            cmd.ExecuteNonQuery();

            con.Close();

        }


        public void AddNewClient(string name, int level, int damage, int health, int dex)
        {
            IDbConnection con = provider.CreateConnection();
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand($"INSERT INTO clients(Name, Level, Damage, Health, Dexterity) VALUES ('{name}', '{level}', '{damage}', '{health}', '{dex}')", (SQLiteConnection)con);
            cmd.ExecuteNonQuery();

            con.Close();
        }

        public Player_Client FindAccount(string name, TcpClient client)
        {
            IDbConnection con = provider.CreateConnection();
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand($"SELECT * from clients WHERE Name = '{name}'", (SQLiteConnection)con);
            SQLiteDataReader reader = cmd.ExecuteReader();
            Player_Client result = mapper.ReadAllClientsFromMapper(reader, client).First();

            con.Close();

            return result;
        }

        public void RemovePlayer(string name, TcpClient client)
        {
            IDbConnection con = provider.CreateConnection();
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand($"DELETE FROM clients WHERE Name = '{name}'", (SQLiteConnection)con);
            cmd.ExecuteNonQuery();

            con.Close();

        }

        public List<Player_Client> GetAllAccounts(TcpClient client)
        {
            IDbConnection con = provider.CreateConnection();
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand("Select * from clients", (SQLiteConnection)con);
            SQLiteDataReader reader = cmd.ExecuteReader();
            List<Player_Client> result = mapper.ReadAllClientsFromMapper(reader, client);

            con.Close();

            return result;
            
        }

        
    }
}
