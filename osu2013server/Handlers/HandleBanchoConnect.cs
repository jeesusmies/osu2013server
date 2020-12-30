using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using osu2013server.Attributes;
using osu2013server.Interfaces;

using static osu2013server.Enums.RequestMethod;

namespace osu2013server.Handlers
{
    [Handler("/web/bancho-connect.php", POST)]
    public class HandleBanchoConnect : IHttpHandler
    {
        public async Task HandleAsync(HttpListenerContext context) { }
    }
}