using System;
using System.IO;
using System.Net;
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
        public async Task HandleAsync(HttpListenerContext context)
        {
            if (context.Request.Headers["User-Agent"] == null)
                return;
            
            context.Response.ContentType = "text/html; charset=UTF-8";
            context.Response.StatusCode = 200;
            context.Response.SendChunked = true;
            context.Response.KeepAlive = true;

            if (context.Request.Headers["osu-token"] == null)
            {
                context.Response.AddHeader("cho-token", Guid.NewGuid().ToString());
                context.Response.AddHeader("cho-protocol", "15");
                
                var credentials = Task.Run(() =>
                {
                    var username = context.Request.InputStream.LimitedReadLine(64);
                    var password = context.Request.InputStream.LimitedReadLine(64);
                    var info = context.Request.InputStream.LimitedReadLine(512);

                    return (username, password, info);
                });

                var player = new Player();

                switch (await player.Authenticate(credentials))
                {
                    case LoginStatus.AuthenticationSuccessful:
                        await context.Response.OutputStream.WriteAsync(new Packets.Out.Login() { Status = 1 }.ReturnPacketBytes());
                        break;
                    
                    case LoginStatus.AuthenticationFailed:
                        await context.Response.OutputStream.WriteAsync(new Packets.Out.Login() { Status = -1 }.ReturnPacketBytes());
                        break;
                    case LoginStatus.TestBuildButNotSupporter:
                        break;
                    case LoginStatus.ServerSideError:
                        break;
                    case LoginStatus.AccountNotActivated:
                        break;
                    case LoginStatus.Banned:
                        break;
                    case LoginStatus.TooOldVersion:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                context.Response.Close();
                return;
            }
            
            using var reader = new BinaryReader(context.Request.InputStream);
            
            /*
            var dsgf = reader.ReadInt16();
            var dfg = reader.ReadByte();
            var hdsgf = reader.ReadInt32();
            var gfds = reader.ReadBytes(hdsgf);
            */
        }
    }
}