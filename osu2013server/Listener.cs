using System;
using System.Dynamic;
using System.Net;
using System.Threading.Tasks;

namespace osu2013server
{
    public class Listener
    {
        private string Prefix { get; set; }
        private static HttpListener HttpListener { get; set; }

        public Listener()
        {
            HttpListener = new();
            HttpListener.Prefixes.Add(Prefix);
        }

        public void Run()
        {
            Task.Run(() =>
            {
                try
                {
                    HttpListener.Start();
                }
                catch (HttpListenerException hlex) when (hlex.ErrorCode == 32 || hlex.ErrorCode == 183)
                {
                    Console.Error.WriteLine("The desired port for the Bancho is in use.");
                    return;
                }
                
                Extension.Log();
            });
        }
    }
}