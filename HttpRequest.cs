using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using HTTPRequest.Exceptions;
using HTTPRequest.Factories;
using HTTPRequest.Model;
using HTTPRequest.Types;

namespace HTTPRequest
{
    public class HttpRequest
    {
        protected HttpClient HttpClient;

        protected HttpRequestConfig HttpRequestConfig;

        protected Dictionary<string, string> RequestHeaders;

        public HttpRequest(HttpRequestConfig config)
        {
            HttpClient = new HttpClient();
            HttpRequestConfig = config;
            RequestHeaders = new Dictionary<string, string>();
        }

        public void AppendHeader(string key, string value)
        {
            if (!RequestHeaders.ContainsKey(key))
                RequestHeaders.Add(key, value);

            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation(key, value);
        }

        public void RemoveHeader(string key)
        {
            if (RequestHeaders.ContainsKey(key))
                RequestHeaders.Remove(key);
        }

        public void ClearHeader()
        {
            RequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Clear();
        }

        public void ChangeTimeoutFromSeconds(int newSeconds)
        {
            HttpClient.Timeout = TimeSpan.FromSeconds(newSeconds);
        }

        public async Task<T> GETAsync<T>(string url)
        {
            using (var http = PrepareRequestHeader())
            {
                return await SendRequest<T>(http, new HttpRequestOptions()
                {
                    Method = HttpMethod.Get,
                    URI = url
                });
            }
        }

        public async Task<T> POSTAsync<T>(string url, string jsonData)
        {
            using (var http = PrepareRequestHeader())
            {
                Uri uri = new Uri(url);
                return await SendRequest<T>(http, new HttpRequestOptions()
                {
                    Method = HttpMethod.Post,
                    URI = uri.AbsoluteUri,
                    HttpContent = CreateBodyRequest(jsonData)
                });
            }
        }

        public async Task<T> POSTAsync<T>(string url, HttpContent httpContent)
        {
            using (var http = PrepareRequestHeader())
            {
                Uri uri = new Uri(url);
                return await SendRequest<T>(http, new HttpRequestOptions()
                {
                    Method = HttpMethod.Post,
                    URI = uri.AbsoluteUri,
                    HttpContent = httpContent
                });
            }
        }

        public async Task<T> PUTAsync<T>(string url)
        {
            using (var http = PrepareRequestHeader())
            {
                Uri uri = new Uri(url);
                return await SendRequest<T>(http, new HttpRequestOptions()
                {
                    Method = HttpMethod.Put,
                    URI = uri.AbsoluteUri,
                    StringContent = null
                });
            }
        }

        public async Task<T> PUTAsync<T>(string url, string jsonData)
        {
            using (var http = PrepareRequestHeader())
            {
                Uri uri = new Uri(url);
                return await SendRequest<T>(http, new HttpRequestOptions()
                {
                    Method = HttpMethod.Put,
                    URI = uri.AbsoluteUri,
                    StringContent = CreateBodyRequest(jsonData)
                });
            }
        }

        public async Task DELETEAsync<T>(string url)
        {
            using (var http = PrepareRequestHeader())
            {
                Uri uri = new Uri(url);
                await SendRequest<T>(http, new HttpRequestOptions()
                {
                    Method = HttpMethod.Delete,
                    URI = uri.AbsoluteUri
                });
            }
        }

        protected async Task<T> SendRequest<T>(HttpClient http, HttpRequestOptions options)
        {
            try
            {
                HttpResponseMessage httpResponse;
                switch (options.Method.Method)
                {
                    case "GET":
                        httpResponse = await http.GetAsync(new Uri(options.URI));
                        break;
                    case "POST":
                        httpResponse = await http.PostAsync(options.URI, options.HttpContent);
                        break;
                    case "PUT":
                        httpResponse = await http.PutAsync(options.URI, options.StringContent);
                        break;
                    case "DELETE":
                        httpResponse = await http.DeleteAsync(options.URI);
                        break;
                    default:
                        throw new HttpUnhandledException(options);
                }

                string json = await httpResponse.Content.ReadAsStringAsync();
                return PrepareResponse<T>(json, httpResponse.StatusCode);
            }
            catch (Exception ex)
            {
                if (ex is TaskCanceledException)
                    throw new HttpTimeoutException($"The request for {options.URI} has reached the waiting limit.", ex);
                else if (((HttpRequestException)ex).GetBaseException() is SocketException)
                    throw new HttpServiceUnavaliableException(((HttpRequestException)ex).GetBaseException().Message, ex);
                else if (((HttpRequestException)ex).GetBaseException() is WebException)
                    throw new HttpConnectionFailureException(((HttpRequestException)ex).GetBaseException().Message, ex);
                else
                    throw new HttpUnhandledException(options, ex);
            }
        }

        protected HttpClient PrepareRequestHeader()
        {
            if (HttpClient == null)
                HttpClient = new HttpClient();

            if (HttpRequestConfig.Authorization == AuthorizationType.BASIC)
            {
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation(HeadersType.Authorization, HttpRequestConfig.GetBasicAuth());
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation(HeadersType.UserAgent, HttpRequestConfig.GetApplicationInfo());
            }
            else if (HttpRequestConfig.Authorization == AuthorizationType.BEARER)
            {
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation(HeadersType.Authorization, HttpRequestConfig.GetBearerAuth());
            }

            if (RequestHeaders.Count > 0)
                foreach (var header in RequestHeaders)
                    if (!HttpClient.DefaultRequestHeaders.Contains(header.Key))
                        HttpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);

            return HttpClient;
        }

        protected StringContent CreateBodyRequest(string jsonData)
        {
            return new StringContent(jsonData, Encoding.UTF8, MediaType.ApplicationJson);
        }

        protected T PrepareResponse<T>(string responseJson, HttpStatusCode statusCode)
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
                        throw new HttpUnauthorizedException("Unauthorized Access");
                    case HttpStatusCode.ServiceUnavailable:
                        throw new HttpServiceUnavaliableException("Service Unavaliable");
                    default:
                        throw new HttpException((int)statusCode, "Unexpected Error", responseJson);
                }
            }
        }
    }
}
