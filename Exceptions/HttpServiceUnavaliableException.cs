using System;

namespace GSD.HTTPRequest.Exceptions
{
    public class HttpServiceUnavaliableException : Exception
    {
        public HttpServiceUnavaliableException(string message) : base(message) { }
    }
}
