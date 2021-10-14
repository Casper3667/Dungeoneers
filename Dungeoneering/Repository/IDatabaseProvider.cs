using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Dungeoneering_Server.Repository
{
    interface IDatabaseProvider
    {
        IDbConnection CreateConnection();
    }
}
