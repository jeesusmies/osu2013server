using System.Threading.Tasks;
using osu2013server.Enums;

using static osu2013server.Enums.LoginStatus;

namespace osu2013server.Objects
{
    public class Player
    {
        public static async Task<LoginStatus> Authenticate(Task<(string username, string password, string info)> credentials)
        {
            var (username, password, info) = credentials.Result;

            return username == "jeesus" ? AuthenticationSuccessful : AuthenticationFailed;
        }
    }
}