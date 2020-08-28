using System;

namespace GSD.HTTPRequest.Exceptions
{
    public class HttpException : Exception
    {
        public int StatusCode { get; set; }

        public string CompleteError { get; set; }

        public HttpException(int statusCode, string message, string error) : base(message)
        {
            StatusCode = statusCode;
            CompleteError = error;
        }
    }
}
