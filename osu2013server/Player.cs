using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using osu2013server.Enums;
using osu2013server.MySql;
using static osu2013server.Enums.LoginStatus;

namespace osu2013server.Objects
{
    public class Player
    {
        public PlayerStats Stats { get; set; }
        public int ID { get; init; }
        public string Username { get; init; }

        public static async Task<(LoginStatus, NameValueCollection)> Authenticate(Task<(string username, string password, string info)> credentials)
        {
            var (username, password, info) = credentials.Result;

            var query = await Player.GetUserAsync(username);
            
            return (query["password"] == password ? AuthenticationSuccessful : AuthenticationFailed, query);
        }
        
        // should these 2 static methods be in Player class?
        // also change sql lib to mysqlconnector
        // to get async methods
        public static async Task<NameValueCollection> GetUserAsync(string username) 
        {
            return Global.Database.Get(
                "select * from (select *, ROW_NUMBER() OVER(order by rankedscore desc) AS 'rank' from osu_users) t WHERE username = @username;", 
                new[] {
                    new MySqlParameter("@username", username),
                });
        }
        
        public static async Task<(float, float)> GetGeoLoc(string ip) 
        {
            var json = await new HttpClient().GetAsync($"http://ip-api.com/json/{ip}");
            
            var items = JsonConvert.DeserializeObject<List<GeoLoc>>(await json.Content.ReadAsStringAsync());

            return (items[0].lat, items[0].lon);
        }
    }
}