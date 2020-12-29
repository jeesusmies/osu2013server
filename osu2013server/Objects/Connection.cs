using System.Collections.Generic;
using System.IO;
using System.Net;

namespace osu2013server
{
    public class Connection
    {
        public Dictionary<string, string> Headers = new();
        public Stream ReqBody { get; set; }

        public void ProcessRequest(HttpListenerRequest request)
        {
            foreach (var key in request.Headers.AllKeys)
            {
                Headers.Add(key, request.Headers.GetValues(key)[0]);
            }

            ReqBody = request.InputStream;
        }
    }
}