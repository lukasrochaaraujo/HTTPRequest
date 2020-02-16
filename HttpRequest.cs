using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using HTTPRequest.Exceptions;
using HTTPRequest.Models;
using HTTPRequest.Types;

namespace HTTPRequest
{
    public class HttpRequest
    {
        private static HttpClient HTTPClient = new HttpClient();
        private string IsoDateTimeFormatString = "yyyy-MM-ddTHH:mm:ss";
        private Dictionary<string, string> RequestHeaders;

        public HttpRequest()
        {
            RequestHeaders = new Dictionary<string, string>();
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

        public void ChangeISODateTimeFormat(string newFormat) => IsoDateTimeFormatString = newFormat;

        public void ResetHTTPClientAndDiscardHeaders()
        {
            HTTPClient = new HttpClient();
            RequestHeaders.Clear();
        }

        public void ResetHTTPClientAndKeepHeaders()
        {
            HTTPClient = new HttpClient();

            foreach (var header in RequestHeaders)
                HTTPClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
        }

        public async Task<T> GETAsync<T>(string url)
        {
            HttpResponseMessage response = await HTTPClient.GetAsync(new Uri(url));
            string json = await response.Content.ReadAsStringAsync();
            return PrepareResponse<T>(json, response.StatusCode);
        }

        public async Task<T> POSTAsync<T>(string url, string jsonData)
        {
            Uri uri = new Uri(url);
            HttpResponseMessage response = await HTTPClient.PostAsync(uri.AbsoluteUri, CreateBodyRequest(jsonData));
            string json = await response.Content.ReadAsStringAsync();
            return PrepareResponse<T>(json, response.StatusCode);
        }

        public async Task<T> PUTAsync<T>(string url)
        {
            Uri uri = new Uri(url);
            HttpResponseMessage response = await HTTPClient.PutAsync(uri.AbsoluteUri, null);
            string json = await response.Content.ReadAsStringAsync();
            return PrepareResponse<T>(json, response.StatusCode);
        }

        public async Task<T> PUTAsync<T>(string url, string jsonData)
        {
            Uri uri = new Uri(url);
            HttpResponseMessage response = await HTTPClient.PutAsync(uri.AbsoluteUri, CreateBodyRequest(jsonData));
            string json = await response.Content.ReadAsStringAsync();
            return PrepareResponse<T>(json, response.StatusCode);
        }

        public async Task DELETEAsync<T>(string url)
        {
            Uri uri = new Uri(url);
            HttpResponseMessage response = await HTTPClient.DeleteAsync(uri.AbsoluteUri);
            string json = await response.Content.ReadAsStringAsync();
            PrepareResponse<T>(json, response.StatusCode);
        }

        private StringContent CreateBodyRequest(string jsonData) => new StringContent(jsonData, Encoding.UTF8, MediaType.ApplicationJson);

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
                    return JsonConvert.DeserializeObject<T>(responseJson, new IsoDateTimeConverter() { DateTimeFormat = IsoDateTimeFormatString });
            }
            else
            {
                if (statusCode == HttpStatusCode.NotFound)
                    return default(T);

                throw new HttpException(responseJson, $"HTTP Status Code: {statusCode.ToString()}");
            }
        }
    }
}
