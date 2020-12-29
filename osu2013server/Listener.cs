using System;
using System.Net;
using System.Threading.Tasks;
using osu2013server.Enums;

namespace osu2013server
{
    public class Listener
    {
        private static HttpListener HttpListener { get; set; }

        public Listener(string prefix)
        {
            HttpListener = new();
            HttpListener.Prefixes.Add(prefix);
        }

        public async Task Run()
        {
            await Task.Run(() =>
            {
                try
                {
                    HttpListener.Start();
                }
                catch (HttpListenerException hlex) when (hlex.ErrorCode == 32 || hlex.ErrorCode == 183)
                {
                    Extension.Log(this, "Port is already being used.", LogStatus.Error);
                    return;
                }
                catch (Exception e)
                {
                    Extension.Log(this, e.ToString(), LogStatus.Error);
                    return;
                }

                Extension.Log(this, "Server now running.", LogStatus.Info);
            });
        }
    }
}