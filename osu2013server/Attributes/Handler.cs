using System.Dynamic;
using osu2013server.Enums;
using osu2013server.Objects;

namespace osu2013server.Attributes
{
    public class Handler : System.Attribute
    {
        public string ID { get; } // example of an ID: /404:GET or /:POST. objects might be a bit messy to use

        public Handler(string route, RequestMethod requestMethod)
        {
            ID = route + ":" + requestMethod;
        }
    }
}