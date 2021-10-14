using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SQLite;

namespace Dungeoneering_Server.Repository
{
    class DatabaseProvider : IDatabaseProvider
    {
        private string connectionString;

        public DatabaseProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public IDbConnection CreateConnection()
        {
            return new SQLiteConnection(connectionString);
        }
    }
}
