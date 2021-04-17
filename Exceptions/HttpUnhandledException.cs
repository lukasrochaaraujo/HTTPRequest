using System;

namespace HTTPRequest.Exceptions
{
    public class HttpUnhandledException : Exception
    {
        public HttpRequestOptions RequestOptions { get; set; }

        public HttpUnhandledException(HttpRequestOptions requestOptions) :
            base(requestOptions.URI)
        {
            RequestOptions = requestOptions;
        }

        public HttpUnhandledException(HttpRequestOptions requestOptions, Exception innerException) : 
            base(requestOptions.URI, innerException) 
        {
            RequestOptions = requestOptions;
        }
    }
}
