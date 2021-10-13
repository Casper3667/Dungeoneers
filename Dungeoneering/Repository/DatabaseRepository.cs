using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Linq;

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

            SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS clients(Name STRING PRIMARY KEY, Level INTEGER, Damage INTEGER, Health INTEGER);", (SQLiteConnection)con);
            cmd.ExecuteNonQuery();

            con.Close();

        }


        public void AddNewClient(string name, int level, int damage, int health)
        {
            IDbConnection con = provider.CreateConnection();
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand($"INSERT INTO clients(Name, Level, Damage, Health) VALUES ('{name}', '{level}', '{damage}', '{health}')", (SQLiteConnection)con);
            cmd.ExecuteNonQuery();

            con.Close();
        }

        public Player_Client FindAccount(string name)
        {
            IDbConnection con = provider.CreateConnection();
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand($"SELECT * from clients Where ID = '{name}'", (SQLiteConnection)con);
            SQLiteDataReader reader = cmd.ExecuteReader();
            Player_Client result = mapper.ReadAllClientsFromMapper(reader).First();

            con.Close();

            return result;
        }
    }
}
