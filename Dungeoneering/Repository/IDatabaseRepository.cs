using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Items;

namespace Dungeoneering_Server.Repository
{
    interface IDatabaseRepository
    {
        void AddNewClient(string name, string password, string salt, int level, int damage, int health, int dex);

        void LevelUp(string name, int level, int damage, int dex, int health);

        void GiveWeapon(string owner, string name, int damage, string element);

        Player_Client FindAccount(string name, TcpClient client);

        void RemovePlayer(string name, TcpClient client);

        void RemoveWeapon(string owner);

        List<Player_Client> GetAllAccounts(TcpClient client);

        List<Weapon> GetAllWeapons();
    }
}
