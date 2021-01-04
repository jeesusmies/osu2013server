using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using osu2013server.MySql;
using osu2013server.Objects;

namespace osu2013server
{
    public static class Global
    {
        public static Dictionary<string, Player> Players = new();
        public static Database Database;
    }
}