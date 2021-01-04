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

            context.Response.ContentType = "text/html; charset=UTF-8";
            context.Response.StatusCode = 200;
            context.Response.SendChunked = true;
            context.Response.KeepAlive = true;

            if (context.Request.Headers["osu-token"] == null)
            {
                context.Response.AddHeader("cho-token", Guid.NewGuid().ToString());
                context.Response.AddHeader("cho-protocol", "15");

                await context.Response.OutputStream.WriteAsync(await Login());
                
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

            switch (authenticationResult)
            {
                case LoginStatus.AuthenticationSuccessful:
                    var player = new Player();

                    writer.Write(new Packets.Out.Login() { Status = int.Parse(query["id"]) }.ToByteArray());

                    break;
                default:
                    writer.Write(new Packets.Out.Login() {  Status = (int) authenticationResult }.ToByteArray());
                    break;
            }

            return response.ToArray();
        }
    }
}