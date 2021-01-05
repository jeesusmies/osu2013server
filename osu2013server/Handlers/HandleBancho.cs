using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using osu2013server.Attributes;
using osu2013server.Enums;
using osu2013server.Interfaces;
using osu2013server.Objects;
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
                        Username = query["username"],
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
                            PerformancePoints = short.Parse("200"),
                            PlayCount = int.Parse(query["playcount"]),
                            Score = int.Parse(query["totalscore"]),
                            Status = Status.Idle
                        }
                    };

                    writer.Write(new Packets.Out.Login() { Status = int.Parse(query["id"]) }.ToByteArray());
                    writer.Write(new Packets.Out.BanchoPrivileges() { Privileges = Privileges.Peppy }.ToByteArray());
                    writer.Write(new Packets.Out.Notification() { Message = "test" }.ToByteArray());
                    
                    writer.Write(new Packets.Out.ChannelJoin() { ChannelName = "#osu"}.ToByteArray());
                    writer.Write(new Packets.Out.ChannelInfo() {Channel = new Channel()
                    {
                        Name = "#osu",
                        Topic = "osuing",
                        PlayerCount = 69
                    }}.ToByteArray());
                    
                    writer.Write(new Packets.Out.UserPresence()
                    {
                        ID = player.ID, Country = 54, Latitude = 1.6f, Longitude = 6.4f, 
                        Privilege = Privileges.Peppy, Username = player.Username, UtcOffset = 3
                    }.ToByteArray());
                    writer.Write(new Packets.Out.UserStats() { Player = player }.ToByteArray());

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