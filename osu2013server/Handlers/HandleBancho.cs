using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using osu2013server.Attributes;
using osu2013server.Enums;
using osu2013server.Interfaces;
using osu2013server.Objects;
using osu2013server.Packets;
using static osu2013server.Enums.RequestMethod;

namespace osu2013server.Handlers
{
    // Post requests in / go here.
    [Handler("/", POST)]
    public class HandleBancho : IHttpHandler
    {
        private HttpListenerContext Context { get; set; }
        
        public async Task HandleAsync(HttpListenerContext context)
        {
            if (context.Request.Headers["User-Agent"] == null)
                return;

            Context = context;

            Context.Response.SuccessfulResponse();

            if (context.Request.Headers["osu-token"] == null)
            {
                context.Response.AddHeader("cho-token", Guid.NewGuid().ToString());
                context.Response.AddHeader("cho-protocol", "15");

                await context.Response.OutputStream.WriteAsync(await Login());
                
                context.Response.Close();
                return;
            }
        }

        private async Task<byte[]> Login()
        {
            var credentials = Task.Run(() =>
            {
                var username = Context.Request.InputStream.LimitedReadLine(64);
                var password = Context.Request.InputStream.LimitedReadLine(64);
                var info = Context.Request.InputStream.LimitedReadLine(512);

                return (username, password, info);
            });

            var response = new MemoryStream();
            await using var writer = new BinaryWriter(response, Encoding.UTF8, true);
            
            var (authenticationResult, query) = await Player.Authenticate(credentials);
            
            var (lon, lat) = await Player.GetGeoLoc(Context.Request.RemoteEndPoint.ToString());
            
            switch (authenticationResult)
            {
                case LoginStatus.AuthenticationSuccessful:
                    var player = new Player()
                    {
                        ID = int.Parse(query["id"]),
                        Stats = new PlayerStats()
                        {
                            Rank = int.Parse(query["rank"]),
                            RankedScore = int.Parse(query["rankedscore"]),
                            Accuracy = int.Parse(query["accuracy"]),
                            Action = "",
                            ActionMD5 = "",
                            Gamemode = Gamemode.Standard,
                            MapID = 0,
                            Mods = Mods.None,
                            PerformancePoints = short.Parse(query["performancepoints"]),
                            PlayCount = int.Parse(query["playcount"]),
                            Score = int.Parse(query["totalscore"]),
                            Status = Status.Idle
                        }
                    };

                    writer.Write(new Packets.Out.Login() { Status = int.Parse(query["id"]) }.ToByteArray());

                    break;
                case LoginStatus.TestBuildButNotSupporter:
                    writer.Write(new Packets.Out.Login() { Status = (int) authenticationResult }.ToByteArray());
                    break;
                case LoginStatus.ServerSideError:
                    writer.Write(new Packets.Out.Login() { Status = (int) authenticationResult }.ToByteArray());
                    break;
                case LoginStatus.AccountNotActivated:
                    writer.Write(new Packets.Out.Login() { Status = (int) authenticationResult }.ToByteArray());
                    break;
                case LoginStatus.Banned:
                    writer.Write(new Packets.Out.Login() { Status = (int) authenticationResult }.ToByteArray());
                    break;
                case LoginStatus.TooOldVersion:
                    writer.Write(new Packets.Out.Login() { Status = (int) authenticationResult }.ToByteArray());
                    break;
                case LoginStatus.AuthenticationFailed:
                    writer.Write(new Packets.Out.Login() { Status = (int) authenticationResult }.ToByteArray());
                    break;
            }

            return response.ToArray();
        }
    }
}