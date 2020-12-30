using System.Dynamic;
using osu2013server.Enums;

namespace osu2013server.Objects
{
    public class RequestInfo
    {
        public string Route { get; set; }
        public RequestMethod RequestMethod { get; set; }
    }
}