using System;
using System.Net;
using System.Threading.Tasks;
using osu2013server.Attributes;
using osu2013server.Interfaces;
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

            if (context.Request.Headers["osu-token"] == null)
            {
                context.Response.AddHeader("osu-token", "0000");
                var truy = context.Request.InputStream.LimitedReadLine(64);
                var fdg = context.Request.InputStream.LimitedReadLine(64);
                var gdsf = context.Request.InputStream.LimitedReadLine(512);

                await context.Response.OutputStream.WriteAsync(new Packets.Out.Login() { Status = 1 }.ReturnPacketBytes());
            }

            context.Response.Close();
        }
    }
}