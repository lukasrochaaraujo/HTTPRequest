using System;

namespace GSD.HTTPRequest.Exceptions
{
    public class HttpUnauthorizedException : Exception
    {
        public HttpUnauthorizedException(string message) : base(message) { }
    }
}
