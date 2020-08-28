using System.Net;

namespace GSD.HTTPRequest.Model
{
    public class HttpResponseModel<T>
    {
        public T Data { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public HttpResponseErrorModel ErrorResponse { get; set; }
    }
}
