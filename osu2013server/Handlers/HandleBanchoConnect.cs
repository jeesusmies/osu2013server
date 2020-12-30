using System.Diagnostics.CodeAnalysis;
using System.Net;
using osu2013server.Attributes;
using osu2013server.Interfaces;

using static osu2013server.Enums.RequestMethod;

namespace osu2013server.Handlers
{
    [Handler("/web/bancho-connect.php", POST)]
    public class HandleBanchoConnect : IHttpHandler
    {
        private HttpListenerResponse Response;
        private HttpListenerRequest Request;
        
        public void Handle(HttpListenerContext context)
        {
            
        }
    }
}