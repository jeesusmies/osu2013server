using System.Net;

namespace osu2013server.Interfaces
{
    public interface IHttpHandler
    {
        private void Handle();
        public void Process(HttpListenerContext context);
    }
}