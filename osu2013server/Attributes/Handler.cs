using osu2013server.Enums;
using osu2013server.Objects;

namespace osu2013server.Attributes
{
    public class Handler : System.Attribute
    {
        public RequestInfo RequestInfo;

        public Handler(string _route, RequestMethod _requestMethod)
        {
            RequestInfo = new RequestInfo() {
                Route = _route,
                RequestMethod = _requestMethod
            };
        }
    }
}