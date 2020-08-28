using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using GSD.HTTPRequest.Enum;
using GSD.HTTPRequest.Exceptions;
using GSD.HTTPRequest.Model;
using GSD.HTTPRequest.Types;
using GSD.HTTPRequest.Util;

namespace GSD.HTTPRequest
{
    public class HttpRequest
    {
        private HttpClientCustom HTTPClient;

        private HttpRequestConfig HttpRequestConfig;

        public HttpContent HttpContent;

        private Dictionary<string, string> RequestHeaders;

        public HttpRequest(HttpRequestConfig config)
        {
            HTTPClient = new HttpClientCustom();
            HttpRequestConfig = config;
            RequestHeaders = new Dictionary<string, string>();
        }

        public void AddNewTimeout(int newSeconds)
        {
            HTTPClient.Timeout = TimeSpan.FromSeconds(newSeconds);
        }

        public void AddHeader(string key, string value)
        {
            if (!RequestHeaders.ContainsKey(key))
                RequestHeaders.Add(key, value);

            HTTPClient.DefaultRequestHeaders.TryAddWithoutValidation(key, value);
        }

        public void RemoveHeader(string key)
        {
            if (RequestHeaders.ContainsKey(key))
                RequestHeaders.Remove(key);
        }

        public void ResetHTTPClientAndDiscardHeaders()
        {
            HTTPClient = new HttpClientCustom();
            RequestHeaders.Clear();
        }

        public void ResetHTTPClientAndKeepHeaders()
        {
            HTTPClient = new HttpClientCustom();

            foreach (var header in RequestHeaders)
                HTTPClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
        }

        public async Task<T> GETAsync<T>(string url)
        {
            using (var http = PrepareRequestHeader())
            {
                HttpResponseMessage response = await SendRequest(http, new HttpRequestOptions()
                {
                    Method = HTTPMethod.GET,
                    URI = url
                });
                string json = await response.Content.ReadAsStringAsync();
                return PrepareResponse<T>(json, response.StatusCode);
            }
        }

        public async Task<T> POSTAsync<T>(string url, string jsonData)
        {
            using (var http = PrepareRequestHeader())
            {
                Uri uri = new Uri(url);
                HttpResponseMessage response;

                HttpContent content;
                if (HttpContent != null)
                    content = HttpContent;
                else
                    content = CreateBodyRequest(jsonData);

                response = await SendRequest(http, new HttpRequestOptions()
                {
                    Method = HTTPMethod.POST,
                    URI = uri.AbsoluteUri,
                    HttpContent = content
                });

                string json = await response.Content.ReadAsStringAsync();

                return PrepareResponse<T>(json, response.StatusCode);
            }
        }

        public async Task<T> POSTAsync<T>(string url, HttpContent httpContent)
        {
            using (var http = PrepareRequestHeader())
            {
                Uri uri = new Uri(url);
                HttpResponseMessage response;

                response = await SendRequest(http, new HttpRequestOptions()
                {
                    Method = HTTPMethod.POST,
                    URI = uri.AbsoluteUri,
                    HttpContent = httpContent
                });

                string json = await response.Content.ReadAsStringAsync();

                return PrepareResponse<T>(json, response.StatusCode);
            }
        }

        public async Task<T> PUTAsync<T>(string url)
        {
            using (var http = PrepareRequestHeader())
            {
                Uri uri = new Uri(url);
                HttpResponseMessage response = await SendRequest(http, new HttpRequestOptions()
                {
                    Method = HTTPMethod.PUT,
                    URI = uri.AbsoluteUri,
                    StringContent = null
                });
                string json = await response.Content.ReadAsStringAsync();
                return PrepareResponse<T>(json, response.StatusCode);
            }
        }

        public async Task<T> PUTAsync<T>(string url, string jsonData)
        {
            using (var http = PrepareRequestHeader())
            {
                Uri uri = new Uri(url);
                HttpResponseMessage response = await SendRequest(http, new HttpRequestOptions()
                {
                    Method = HTTPMethod.PUT,
                    URI = uri.AbsoluteUri,
                    StringContent = CreateBodyRequest(jsonData)
                });
                string json = await response.Content.ReadAsStringAsync();
                return PrepareResponse<T>(json, response.StatusCode);
            }
        }

        public async Task DELETEAsync<T>(string url)
        {
            using (var http = PrepareRequestHeader())
            {
                Uri uri = new Uri(url);
                HttpResponseMessage response = await SendRequest(http, new HttpRequestOptions()
                {
                    Method = HTTPMethod.DELETE,
                    URI = uri.AbsoluteUri
                });
                string json = await response.Content.ReadAsStringAsync();
                PrepareResponse<T>(json, response.StatusCode);
            }
        }

        protected async Task<HttpResponseMessage> SendRequest(HttpClient http, HttpRequestOptions options)
        {
            try
            {
                switch (options.Method)
                {
                    case HTTPMethod.GET:
                        return await http.GetAsync(new Uri(options.URI));
                    case HTTPMethod.POST:
                        return await http.PostAsync(options.URI, options.HttpContent);
                    case HTTPMethod.PUT:
                        return await http.PutAsync(options.URI, options.StringContent);
                    case HTTPMethod.DELETE:
                        return await http.DeleteAsync(options.URI);
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                if (ex is TaskCanceledException)
                    throw new HttpTimeoutException($"A requisição para {options.URI} atingiu o limite de espera.");
                else if (((HttpRequestException)ex).GetBaseException() is SocketException)
                    throw new HttpServiceUnavaliableException(((HttpRequestException)ex).GetBaseException().Message);
                else if (((HttpRequestException)ex).GetBaseException() is WebException)
                    throw new HttpConnectionFailureException(((HttpRequestException)ex).GetBaseException().Message);
                else
                    throw;
            }
        }

        private HttpClient PrepareRequestHeader()
        {
            if (HTTPClient == null || HTTPClient.IsDisposed)
                HTTPClient = new HttpClientCustom();

            if (HttpRequestConfig.Authorization == AuthorizationType.BASIC)
            {
                HTTPClient.DefaultRequestHeaders.TryAddWithoutValidation(HeadersType.Authorization, HttpRequestConfig.GetBasicAuthHash());
                HTTPClient.DefaultRequestHeaders.TryAddWithoutValidation(HeadersType.UserAgent, HttpRequestConfig.GetApplicationInfo());
            }
            else if (HttpRequestConfig.Authorization == AuthorizationType.BEARER)
            {
                HTTPClient.DefaultRequestHeaders.TryAddWithoutValidation(HeadersType.Authorization, HttpRequestConfig.GetBearerAuth());
            }

            if (RequestHeaders.Count > 0)
                foreach (var header in RequestHeaders)
                    if (!HTTPClient.DefaultRequestHeaders.Contains(header.Key))
                        HTTPClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);

            return HTTPClient;
        }

        private StringContent CreateBodyRequest(string jsonData)
        {
            return new StringContent(jsonData, Encoding.UTF8, MediaType.ApplicationJson);
        }

        private T PrepareResponse<T>(string responseJson, HttpStatusCode statusCode)
        {
            if ((int)statusCode >= (int)HttpStatusCode.OK && (int)statusCode < (int)HttpStatusCode.MultipleChoices)
            {
                if (typeof(T) == typeof(BasicResponseModel))
                {
                    var responseModel = new BasicResponseModel()
                    {
                        StatusCode = (int)statusCode,
                        JsonBody = responseJson
                    };

                    return (T)Convert.ChangeType(responseModel, typeof(T));
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(responseJson))
                        return default(T);
                    else if (responseJson == "[]" || responseJson == "{}")
                        return (T)Activator.CreateInstance(typeof(T));
                    else
                        return JsonConvert.DeserializeObject<T>(responseJson, IsoDateTimeFactory.Create(HttpRequestConfig.IsoDateTimeFormat));
                }
            }
            else
            {
                switch (statusCode)
                {
                    case HttpStatusCode.NotFound:
                        return default(T);
                    case HttpStatusCode.Unauthorized:
                        throw new HttpUnauthorizedException("Acesso não autorizado");
                    case HttpStatusCode.ServiceUnavailable:
                        throw new HttpServiceUnavaliableException("Serviço temporariamente indisponível! Tente novamente em instantes!");
                    default:
                        throw new HttpException((int)statusCode, "O serviço apresentou um erro!\nContate o TI!", responseJson);
                }
            }
        }
    }
}
