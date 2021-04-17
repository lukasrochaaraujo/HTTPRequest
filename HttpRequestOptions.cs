using System.Net.Http;

namespace HTTPRequest
{
    public class HttpRequestOptions
    {
        public HttpMethod Method { get; set; }

        public string URI { get; set; }

        public string StringJson { get; set; }

        public StringContent StringContent { get; set; }

        public HttpContent HttpContent { get; set; }
    }
}
