using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeoneering_Server.Repository
{
    interface IDatabaseRepository
    {
        void AddNewClient(string name, int level, int damage, int health);

        Player_Client FindAccount(string name);
    }
}
