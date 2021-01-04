using System.Collections.Specialized;
using System.Threading.Tasks;
using osu2013server.Enums;

using static osu2013server.Enums.LoginStatus;

namespace osu2013server.Objects
{
    public class Player
    {


        public static async Task<(LoginStatus, NameValueCollection)> Authenticate(Task<(string username, string password, string info)> credentials)
        {
            var (username, password, info) = credentials.Result;

            var query = await Global.GetUserAsync(username);
            
            return (query["password"] == password ? AuthenticationSuccessful : AuthenticationFailed, query);
        }
    }
}