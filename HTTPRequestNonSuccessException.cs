using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

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

