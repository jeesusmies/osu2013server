using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using osu2013server.Attributes;
using osu2013server.Enums;
using osu2013server.Interfaces;
using osu2013server.Objects;

namespace osu2013server
{
    public class Listener
    {
        private static HttpListener HttpListener { get; set; }
        private static Dictionary<RequestInfo, IHttpHandler> _handlers = new();

        public Listener(string prefix)
        {
            HttpListener = new();
            HttpListener.Prefixes.Add(prefix);
            AddListeningRoutes();
        }

        public async Task Run()
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
            catch (HttpListenerException hlex) when (hlex.ErrorCode == 5)
            {
                Extension.Log(this, "Server needs administrator privileges to run.", LogStatus.Error);
                return;
            }
            catch (Exception e)
            {
                Extension.Log(this, e.ToString(), LogStatus.Error);
                return;
            }

            Extension.Log(this, "Server now running.", LogStatus.Info);

            while (true)
            { 
                var ctx = await HttpListener.GetContextAsync();
                Task.Factory.StartNew(async () => { await ProcessContextAsync(ctx); });
                
            }
        }

        public async Task ProcessContextAsync(HttpListenerContext ctx)
        {
            Stopwatch stopwatch = new();
        }

        private void AddListeningRoutes()
        {
            Assembly info = Assembly.GetExecutingAssembly();

            foreach(var type in info.GetTypes())
            {
                if (Attribute.IsDefined(type, typeof(Handler)))
                {
                    var att = (Handler) Attribute.GetCustomAttribute(type, typeof(Handler));
                    _handlers.Add(att.RequestInfo, (IHttpHandler)Activator.CreateInstance(type));
                }
            }
        }
    }
}