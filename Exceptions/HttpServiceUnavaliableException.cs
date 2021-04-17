using System;

namespace HTTPRequest.Exceptions
{
    public class HttpServiceUnavaliableException : Exception
    {
        public HttpServiceUnavaliableException(string message) : base(message) { }

        public HttpServiceUnavaliableException(string message, Exception innerException) : 
            base(message, innerException) { }
    }
}
