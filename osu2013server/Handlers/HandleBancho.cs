﻿using System.Net;
using osu2013server.Attributes;
using osu2013server.Interfaces;

using static osu2013server.Enums.HttpRequestType;

namespace osu2013server.Handlers
{
    // Post requests in / go here.
    [Handler("/", POST)]
    public class HandleBancho : IHttpHandler
    {
        private HttpListenerResponse Response;
        private HttpListenerRequest Request;
        
        private void Handle()
        {
            throw new System.NotImplementedException();
        }

        public void Process(HttpListenerContext context)
        {
            
        }
    }
}