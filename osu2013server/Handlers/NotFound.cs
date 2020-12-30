using System.Net;
using System.Text;
using System.Threading.Tasks;
using osu2013server.Attributes;
using osu2013server.Interfaces;
using static osu2013server.Enums.RequestMethod;

namespace osu2013server.Handlers
{
    [Handler("/404", GET)]
    public class NotFound : IHttpHandler
    {
        public async Task HandleAsync(HttpListenerContext context)
        {
            context.Response.StatusCode = 404;
            
            byte[] resp = Encoding.UTF8.GetBytes("fuck u");
            await context.Response.OutputStream.WriteAsync(resp);
            context.Response.Close();
        }
    }
}