using System;

namespace osu2013server.Enums
{
    [Flags]
    public enum RequestMethod
    {
        GET, 
        HEAD,
        POST, 
        PUT, 
        DELETE, 
        CONNECT, 
        OPTIONS,
        TRACE, 
        PATCH,
        
    }
}