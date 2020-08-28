using System;

namespace GSD.HTTPRequest.Exceptions
{
    public class HttpConnectionFailureException : Exception
    {
        public HttpConnectionFailureException(string message) : base(message) { }
    }
}
