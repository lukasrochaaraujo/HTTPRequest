using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using HTTPRequest.Exception;
using HTTPRequest.Model;
using HTTPRequest.Types;
using HTTPRequest.Util;

namespace HTTPRequest
{
    public class HttpRequest
    {
        private HttpRequestConfig RequestConfiguration;
        private Dictionary<string, string> RequestHeaders;

        public HttpRequest(HttpRequestConfig config)
        {
            RequestConfiguration = config;
            RequestHeaders = new Dictionary<string, string>();
        }

        public void AppendHeader(string key, string value)
        {
            RequestHeaders.Add(key, value);
        }

        public async Task<T> GETAsync<T>(string url)
        {
            using (var http = PrepareRequestHeader())
            {
                HttpResponseMessage response = await http.GetAsync(new Uri(url));
                string json = await response.Content.ReadAsStringAsync();
                return PrepareResponse<T>(json, response.StatusCode);
            }
        }

        public async Task<T> POSTAsync<T>(string url, string jsonData)
        {
            using (var http = PrepareRequestHeader())
            {
                Uri uri = new Uri(url);
                HttpResponseMessage response = await http.PostAsync(uri.AbsoluteUri, CreateBodyRequest(jsonData));
                string json = await response.Content.ReadAsStringAsync();
                return PrepareResponse<T>(json, response.StatusCode);
            }
        }

        public async Task<T> PUTAsync<T>(string url)
        {
            using (var http = PrepareRequestHeader())
            {
                Uri uri = new Uri(url);
                HttpResponseMessage response = await http.PutAsync(uri.AbsoluteUri, null);
                string json = await response.Content.ReadAsStringAsync();
                return PrepareResponse<T>(json, response.StatusCode);
            }
        }

        public async Task<T> PUTAsync<T>(string url, string jsonData)
        {
            using (var http = PrepareRequestHeader())
            {
                Uri uri = new Uri(url);
                HttpResponseMessage response = await http.PutAsync(uri.AbsoluteUri, CreateBodyRequest(jsonData));
                string json = await response.Content.ReadAsStringAsync();
                return PrepareResponse<T>(json, response.StatusCode);
            }
        }

        public async Task DELETEAsync<T>(string url)
        {
            using (var http = PrepareRequestHeader())
            {
                Uri uri = new Uri(url);
                HttpResponseMessage response = await http.DeleteAsync(uri.AbsoluteUri);
                string json = await response.Content.ReadAsStringAsync();
                PrepareResponse<T>(json, response.StatusCode);
            }
        }

        private HttpClient PrepareRequestHeader()
        {
            HttpClient http = new HttpClient();
            http.DefaultRequestHeaders.TryAddWithoutValidation(HeadersType.Authorization, RequestConfiguration.GetBasicAuthHash());
            http.DefaultRequestHeaders.TryAddWithoutValidation(HeadersType.UserAgent, RequestConfiguration.GetApplicationInfo());

            foreach (var header in RequestHeaders)
                http.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);

            return http;
        }

        private StringContent CreateBodyRequest(string jsonData)
        {
            return new StringContent(jsonData, Encoding.UTF8, MediaType.ApplicationJson);
        }

        private T PrepareResponse<T>(string responseJson, HttpStatusCode statusCode)
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

            if ((int)statusCode >= (int)HttpStatusCode.OK && (int)statusCode < (int)HttpStatusCode.MultipleChoices)
            {
                if (string.IsNullOrWhiteSpace(responseJson))
                    return default(T);
                else if (responseJson == "[]")
                    return (T)Activator.CreateInstance(typeof(T));
                else
                    return JsonConvert.DeserializeObject<T>(responseJson, IsoDateTimeFactory.Create(RequestConfiguration.IsoDateTimeFormat));
            }
            else
            {
                if (statusCode == HttpStatusCode.NotFound)
                    return default(T);

                if (statusCode == HttpStatusCode.Unauthorized)
                    throw new HttpException(new HttpResponseErrorModel() { Status = (int)statusCode }, "HTTP 401: Unauthorized");

                if (statusCode == HttpStatusCode.ServiceUnavailable)
                    throw new HttpServiceUnavaliableException("HTTP 503: Service Unavailable");

                var errorResponse = JsonConvert.DeserializeObject<HttpResponseErrorModel>(responseJson);
                throw new HttpException(errorResponse, (errorResponse?.Message ?? "Unknown Error"));
            }
        }
    }
}
