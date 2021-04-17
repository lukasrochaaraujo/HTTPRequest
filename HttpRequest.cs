using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using HTTPRequest.Model;

namespace HTTPRequest
{
    public class HttpRequest
    {
        protected readonly HttpClient HttpClient;

        protected readonly HttpRequestOptions HttpRequestOptions;

        public HttpRequest(HttpRequestOptions options = null)
        {
            HttpRequestOptions = options ?? new HttpRequestOptions();
            HttpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(HttpRequestOptions.TimeOutInSeconds)
            };
        }

        public void AppendHeader(string key, string value)
            => HttpClient.DefaultRequestHeaders.TryAddWithoutValidation(key, value);

        public void RemoveHeader(string key)
            => HttpClient.DefaultRequestHeaders.Remove(key);

        public void ClearHeaders()
            => HttpClient.DefaultRequestHeaders.Clear();

        public async Task<T> GETAsync<T>(string url)
            => await SendRequest<T>(HttpMethod.Get, url);

        public async Task<T> POSTAsync<T>(string url, string jsonBody)
            => await SendRequest<T>(HttpMethod.Post, url, jsonBody);

        public async Task<T> PUTAsync<T>(string url)
            => await SendRequest<T>(HttpMethod.Put, url);

        public async Task<T> PUTAsync<T>(string url, string jsonBody)
            => await SendRequest<T>(HttpMethod.Put, url, jsonBody);

        public async Task DELETEAsync<T>(string url)
            => await SendRequest<T>(HttpMethod.Delete, url);

        protected async Task<T> SendRequest<T>(HttpMethod method, string url, string jsonBody = null)
        {
            HttpResponseMessage httpResponse = null;
            try
            {
                var requestUri = new Uri(url);
                var requestContent = ConvertJsonToStringContent(jsonBody);
                int currentRequestTentative = 1;

                for(;;)
                {
                    try
                    {
                        switch (method.Method)
                        {
                            case "GET":
                                httpResponse = await HttpClient.GetAsync(requestUri.AbsoluteUri);
                                break;
                            case "POST":
                                httpResponse = await HttpClient.PostAsync(requestUri.AbsoluteUri, requestContent);
                                break;
                            case "PUT":
                                httpResponse = await HttpClient.PutAsync(requestUri.AbsoluteUri, requestContent);
                                break;
                            case "DELETE":
                                httpResponse = await HttpClient.DeleteAsync(requestUri.AbsoluteUri);
                                break;
                            default:
                                throw new NotSupportedException($"Method {method.Method} not supported. Try GET, POST, PUT or DELETE.");
                        }

                        httpResponse.EnsureSuccessStatusCode();
                        break;
                    }
                    catch
                    {
                        if (currentRequestTentative++ < HttpRequestOptions.MaxRequestAttempts)
                            continue;

                        throw;
                    }
                }

                string json = await httpResponse.Content.ReadAsStringAsync();
                return PrepareResponse<T>(json, httpResponse.StatusCode);
            }
            catch (Exception ex)
            {
                throw new HTTPRequestNonSuccessException(httpResponse, ex);
            }
        }

        protected StringContent ConvertJsonToStringContent(string jsonData)
        {
            if (string.IsNullOrWhiteSpace(jsonData))
                return null;

            return new StringContent(jsonData, Encoding.UTF8, "application/json");
        }

        protected T PrepareResponse<T>(string responseJson, HttpStatusCode statusCode)
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
                    return JsonConvert.DeserializeObject<T>(responseJson, new IsoDateTimeConverter() { DateTimeFormat = HttpRequestOptions.DateTimeFormat });
            }
        }
    }
}
