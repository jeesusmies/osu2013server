using osu2013server.Enums;

namespace osu2013server.Attributes
{
    public class Handler : System.Attribute
    {
        public string location;
        public HttpRequestType requestType;

        public Handler(string location, HttpRequestType requestType)
        {
            location = this.location;
            requestType = this.requestType;
        }
    }
}