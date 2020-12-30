using System;
using osu2013server.Enums;

using static osu2013server.Enums.LogStatus;

namespace osu2013server
{
    public static class Extension
    {
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
        
        public static T ToEnum<T>(this string value)
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }
    }
}