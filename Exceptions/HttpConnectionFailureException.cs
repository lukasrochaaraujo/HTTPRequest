using System;

namespace HTTPRequest.Exceptions
{
    public class HttpConnectionFailureException : Exception
    {
        public HttpConnectionFailureException(string message, Exception innerException) : 
            base(message, innerException) { }
    }
}
