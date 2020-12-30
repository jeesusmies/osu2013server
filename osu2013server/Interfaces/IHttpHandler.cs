using System.Net;
using System.Threading.Tasks;

namespace osu2013server.Interfaces
{
    public interface IHttpHandler
    {
        public Task HandleAsync(HttpListenerContext context);
    }
}