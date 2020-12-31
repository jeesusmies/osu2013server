using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using osu2013server.Attributes;
using osu2013server.Interfaces;

using static osu2013server.Enums.RequestMethod;

namespace osu2013server.Handlers
{
    [Handler("/web/bancho_connect.php", GET)]
    public class HandleBanchoConnect : IHttpHandler
    {
        public async Task HandleAsync(HttpListenerContext context)
        {
            context.Response.StatusCode = 200;

            var resp = Encoding.UTF8.GetBytes("ca");
            
            context.Response.OutputStream.Write(resp);
            context.Response.Close();
        }
    }
}