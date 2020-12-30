using osu2013server.Enums;

namespace osu2013server.Attributes
{
    public class Handler : System.Attribute
    {
        public string location;
        public HttpRequestType requestType;

        public Handler(string _location, HttpRequestType _requestType)
        {
            location = _location;
            requestType = _requestType;
        }
    }
}