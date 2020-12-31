using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
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
        internal static Dictionary<string, IHttpHandler> _handlers = new();

        public Listener(string prefix)
        {
            HttpListener = new();
            HttpListener.Prefixes.Add(prefix);
            Extension.AddListeningRoutes();
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
                await ProcessContextAsync(ctx).ConfigureAwait(false);
                
            }
        }

        public async Task ProcessContextAsync(HttpListenerContext ctx)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            var request = ctx.Request;

            
            Console.WriteLine("URL: {0}", request.Url.OriginalString);
            Console.WriteLine("Raw URL: {0}", request.RawUrl);
            Console.WriteLine("Query: {0}", request.QueryString);

            // Display the referring URI.
            Console.WriteLine("Referred by: {0}", request.UrlReferrer);

            //Display the HTTP method.
            Console.WriteLine("HTTP Method: {0}", request.HttpMethod);
            //Display the host information specified by the client;
            Console.WriteLine("Host name: {0}", request.UserHostName);
            Console.WriteLine("Host address: {0}", request.UserHostAddress);
            Console.WriteLine("User agent: {0}", request.UserAgent);
            
            
            try
            {
                await _handlers[request.ToID()].HandleAsync(ctx);
            }
            catch (KeyNotFoundException knfe)
            {
                Extension.Log(this, $"{request.UserHostAddress} Requested to non-existant route [{new Uri(request.Url.OriginalString).AbsolutePath} {request.HttpMethod}]", LogStatus.Warning);
                ctx.Response.Redirect("/404");
                ctx.Response.Close();
                return;
            }

            stopwatch.Stop();
            
            Extension.Log(this, $"Request to [{request.RawUrl}] took {stopwatch.Elapsed.Milliseconds}ms", LogStatus.Info);
        }
    }
}