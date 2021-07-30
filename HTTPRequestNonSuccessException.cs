using System;
using System.Net.Http;

namespace HTTPRequest
{
    public class HTTPRequestNonSuccessException : Exception
    {
        public HttpResponseMessage HttpReponse { get; private set; }

        public HTTPRequestNonSuccessException(HttpResponseMessage httpResponse, Exception innerException)
            :base(innerException.Message, innerException)
        {
            HttpReponse = httpResponse;
        }
    }
}

