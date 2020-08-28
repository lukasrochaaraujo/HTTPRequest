using System.Net.Http;

namespace GSD.HTTPRequest
{
    public class HttpClientCustom : HttpClient
    {
        public bool IsDisposed { get; set; }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = disposing;
            base.Dispose(disposing);
        }
    }
}
