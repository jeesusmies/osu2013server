using System.IO;
using System.Net;
using System.Threading.Tasks;
using osu2013server.Attributes;
using osu2013server.Interfaces;

using static osu2013server.Enums.RequestMethod;

namespace osu2013server.Handlers
{
    [Handler("/", GET)]
    public class HandleMOTD : IHttpHandler
    {
        public async Task HandleAsync(HttpListenerContext context)
        {
            await context.Response.OutputStream.WriteAsync(await File.ReadAllBytesAsync(@"../../../MOTD.txt"));
            context.Response.Close();
        }
    }
}