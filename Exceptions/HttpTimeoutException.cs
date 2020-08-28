using System;

namespace GSD.HTTPRequest.Exceptions
{
    public class HttpTimeoutException : Exception
    {
        public HttpTimeoutException(string message) : base(message) { }
    }
}
