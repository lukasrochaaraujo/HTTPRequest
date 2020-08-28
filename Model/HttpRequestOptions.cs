using System.Net.Http;

using GSD.HTTPRequest.Enum;

namespace GSD.HTTPRequest.Model
{
    public class HttpRequestOptions
    {
        public HTTPMethod Method { get; set; }

        public string URI { get; set; }

        public string StringJson { get; set; }

        public StringContent StringContent { get; set; }

        public HttpContent HttpContent { get; set; }
    }
}
