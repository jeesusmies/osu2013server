﻿using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using osu2013server.Attributes;
using osu2013server.Enums;
using osu2013server.Interfaces;
using static osu2013server.Enums.LogStatus;

namespace osu2013server
{
    public static class Extension
    {
        public static string LimitedReadLine(this Stream stream, uint limit)
        {
            var sb = new StringBuilder();

            int c;
            while (limit-- > 0 && (c = stream.ReadByte()) != '\n') { 
                sb.Append((char)c);
            }

            return sb.ToString();
        }
        
        public static void Log(dynamic obj, string message, LogStatus status)
        {
            Console.ForegroundColor = status switch
            {
                Info => ConsoleColor.Green,
                Warning => ConsoleColor.Yellow,
                Error => ConsoleColor.Red,
            };

            Console.WriteLine($@"[{obj.GetType()}] {message}");
        }
        
        public static void WriteBString(this BinaryWriter writer, string s)
        {
            if (s.Length == 0)
            { 
                writer.Write((byte)0x00);
                return;
            }

            writer.Write((byte)11);
            writer.Write(s);
        }
        
        public static T ToEnum<T>(this string value)
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }
        
        public static string ToID(this HttpListenerRequest request)
        {
            return new Uri(request.Url.OriginalString).AbsolutePath + ":" + request.HttpMethod;
        }

        public static void SuccessfulResponse(this HttpListenerResponse response)
        {
            response.ContentType = "application/octet-stream";
            response.StatusCode = 200;
            response.SendChunked = true;
            response.KeepAlive = true;
        }

        public static void AddListeningRoutes()
        {
            Assembly info = Assembly.GetExecutingAssembly();

            foreach(var type in info.GetTypes())
            {
                if (Attribute.IsDefined(type, typeof(Handler)))
                {
                    var att = (Handler) Attribute.GetCustomAttribute(type, typeof(Handler));
                    Listener._handlers.Add(att.ID, (IHttpHandler)Activator.CreateInstance(type));
                }
            }
        }
    }
}