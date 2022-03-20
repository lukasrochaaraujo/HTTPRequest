using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HTTPRequest
{
    /// <summary>
    /// Performs requests HTTP with the main verbs and using the JSON format
    /// </summary>
    public class HttpRequest
    {
        protected readonly HttpClient HttpClient;

        protected readonly HttpRequestOptions HttpRequestOptions;

        /// <summary>
        /// Starts a new instance with the (optional) settings provided.
        /// </summary>
        /// <param name="options">Settings to be applied</param>
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

        public async Task<TReturnType> GETAsync<TReturnType>(string url)
            => await SendRequest<TReturnType>(HttpMethod.Get, url);

        public async Task<TReturnType> POSTAsync<TReturnType>(string url, string jsonBody)
            => await SendRequest<TReturnType>(HttpMethod.Post, url, jsonBody);

        public async Task<TReturnType> PUTAsync<TReturnType>(string url)
            => await SendRequest<TReturnType>(HttpMethod.Put, url);

        public async Task<TReturnType> PUTAsync<TReturnType>(string url, string jsonBody)
            => await SendRequest<TReturnType>(HttpMethod.Put, url, jsonBody);

        public async Task DELETEAsync<TReturnType>(string url)
            => await SendRequest<TReturnType>(HttpMethod.Delete, url);

        protected async Task<TReturnType> SendRequest<TReturnType>(HttpMethod method, string url, string jsonBody = null)
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
                        {
                            await Task.Delay(TimeSpan.FromSeconds(HttpRequestOptions.IntevalBetweenAttemptsInSeconds));
                            continue;
                        }

                        throw;
                    }
                }

                string json = await httpResponse.Content.ReadAsStringAsync();
                return PrepareResponse<TReturnType>(json, httpResponse.StatusCode);
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

        protected TResponseType PrepareResponse<TResponseType>(string responseJson, HttpStatusCode statusCode)
        {
            if (typeof(TResponseType) == typeof(BasicResponseModel))
            {
                var responseModel = new BasicResponseModel()
                {
                    StatusCode = (int)statusCode,
                    ResponseBody = responseJson
                };

                return (TResponseType)Convert.ChangeType(responseModel, typeof(TResponseType));
            }
            else
            {
                if (string.IsNullOrWhiteSpace(responseJson))
                    return default(TResponseType);
                else if (responseJson == "[]" || responseJson == "{}")
                    return (TResponseType)Activator.CreateInstance(typeof(TResponseType));
                else
                    return JsonConvert.DeserializeObject<TResponseType>(responseJson, new IsoDateTimeConverter() { DateTimeFormat = HttpRequestOptions.DateTimeFormat });
            }
        }
    }
}
