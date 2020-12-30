﻿using System;
using System.Net;
using osu2013server.Attributes;
using osu2013server.Interfaces;

using static osu2013server.Enums.HttpRequestType;

namespace osu2013server.Handlers
{
    // Post requests in / go here.
    [Handler("/", POST)]
    public class HandleBancho : IHttpHandler
    {
        public static void Handle(HttpListenerContext context)
        {
            if (context.Request.Headers["User-Agent"] == null)
                return;

            if (context.Request.Headers["osu-token"] == null)
                context.Response.AddHeader("osu-token", "0000");
        }
    }
}