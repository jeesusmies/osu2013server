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

        // change sql lib to mysqlconnector
        // to get async methods
        public static async Task<NameValueCollection> GetUserAsync(string username) 
        {
            return Database.Get(
                "select * from (select *, ROW_NUMBER() OVER(order by rankedscore desc) AS 'rank' from osu_users) t WHERE username = @username;", 
                new[] {
                    new MySqlParameter("@username", username),
                });
        }
    }
}