using System;

namespace HTTPRequest.Exceptions
{
    public class HttpTimeoutException : Exception
    {
        public HttpTimeoutException(string message, Exception innerException) : 
            base(message, innerException) { }
    }
}
