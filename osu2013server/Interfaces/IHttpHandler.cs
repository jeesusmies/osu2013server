using System.Net;

namespace osu2013server.Interfaces
{
    public interface IHttpHandler
    {
        public void Handle(HttpListenerContext context);
    }
}