using System.Net;

namespace HTTPRequest.Model
{
    public class HttpResponseModel<T>
    {
        public T Data { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public HttpResponseErrorModel ErrorResponse { get; set; }
    }
}
