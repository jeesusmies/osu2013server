using System.Diagnostics.CodeAnalysis;
using System.Net;
using osu2013server.Attributes;
using osu2013server.Interfaces;

using static osu2013server.Enums.HttpRequestType;

namespace osu2013server.Handlers
{
    [Handler("/web/bancho-connect.php", POST)]
    public class HandleBanchoConnect : IHttpHandler
    {
        private void Handle()
        {
            throw new System.NotImplementedException();
        }

        public void Process(HttpListenerContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}