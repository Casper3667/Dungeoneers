using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using Items;

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

            //SQLiteCommand cmd = new SQLiteCommand("DROP TABLE items", (SQLiteConnection)con);
            //cmd.ExecuteNonQuery();

            SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS clients(Name STRING PRIMARY KEY, Password STRING, Salt STRING, Level INTEGER, Damage INTEGER, Health INTEGER, Dexterity INTEGER);", (SQLiteConnection)con);
            cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS items(Owner STRING PRIMARY KEY, Damage INTEGER, Element STRING, Name STRING );", (SQLiteConnection)con);
            cmd.ExecuteNonQuery();

            con.Close();

        }


        public void AddNewClient(string name, string password, string salt, int level, int damage, int health, int dex)
        {
            IDbConnection con = provider.CreateConnection();
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand($"INSERT INTO clients(Name, Password, Salt, Level, Damage, Health, Dexterity) VALUES ('{name}', '{password}', '{salt}', '{level}', '{damage}', '{health}', '{dex}')", (SQLiteConnection)con);
            cmd.ExecuteNonQuery();

            con.Close();
        }

        public void GiveWeapon(string owner, string name, int damage, string element)
        {
            IDbConnection con = provider.CreateConnection();
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand($"INSERT INTO items(Owner, Damage, Element, Name) VALUES ('{owner}', '{damage}', '{element}', '{name}')", (SQLiteConnection)con);
            cmd.ExecuteNonQuery();

            con.Close();
        }

        public void LevelUp(string name, int level, int dmg, int dex, int health)
        {
            IDbConnection con = provider.CreateConnection();
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand($"UPDATE clients SET(Level, Damage, Dexterity, Health) = ('{level}', '{dmg}', '{dex}', '{health}') WHERE Name = '{name}'", (SQLiteConnection)con);
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

        public void RemoveWeapon(string owner)
        {
            IDbConnection con = provider.CreateConnection();
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand($"DELETE FROM items WHERE Owner = '{owner}'", (SQLiteConnection)con);
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

        public List<Weapon> GetAllWeapons()
        {
            IDbConnection con = provider.CreateConnection();
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand("Select * from items", (SQLiteConnection)con);
            SQLiteDataReader reader = cmd.ExecuteReader();
            List<Weapon> result = mapper.ReadAllWeaponsFromMapper(reader);

            con.Close();

            return result;
        }

        
    }
}
