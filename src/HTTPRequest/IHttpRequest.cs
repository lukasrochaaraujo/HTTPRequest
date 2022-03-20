using System.Threading.Tasks;

namespace HTTPRequest
{
    /// <summary>
    /// Performs requests HTTP with the main verbs and using the JSON format
    /// </summary>
    public interface IHttpRequest
    {
        /// <summary>
        /// Add an key/value on request header
        /// </summary>
        void AppendHeader(string key, string value);

        /// <summary>
        /// Remove an key/value on request header by key
        /// </summary>
        void RemoveHeader(string key);

        /// <summary>
        /// Reset header to default
        /// </summary>
        void ClearHeaders();

        /// <summary>
        /// Send a HTTP GET request
        /// </summary>
        /// <typeparam name="TReturnType">Type that will be converted from json</typeparam>
        /// <param name="url">Target URL</param>
        /// <returns>Type that will be converted from json</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="HTTPRequestNonSuccessException"></exception>
        Task<TReturnType> GETAsync<TReturnType>(string url);

        /// <summary>
        /// Send a HTTP GET request
        /// </summary>
        /// <typeparam name="TReturnType">Type that will be converted from json</typeparam>
        /// <param name="url">Target URL</param>
        /// <param name="jsonBody">JSON to send in request body</param>
        /// <returns>Type that will be converted from json</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="HTTPRequestNonSuccessException"></exception>
        Task<TReturnType> POSTAsync<TReturnType>(string url, string jsonBody);

        /// <summary>
        /// Send a HTTP GET request
        /// </summary>
        /// <typeparam name="TReturnType">Type that will be converted from json</typeparam>
        /// <param name="url">Target URL</param>
        /// <returns>Type that will be converted from json</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="HTTPRequestNonSuccessException"></exception>
        Task<TReturnType> PUTAsync<TReturnType>(string url);

        /// <summary>
        /// Send a HTTP GET request
        /// </summary>
        /// <typeparam name="TReturnType">Type that will be converted from json</typeparam>
        /// <param name="url">Target URL</param>
        /// <param name="jsonBody">JSON to send in request body</param>
        /// <returns>Type that will be converted from json</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="HTTPRequestNonSuccessException"></exception>
        Task<TReturnType> PUTAsync<TReturnType>(string url, string jsonBody);

        /// <summary>
        /// Send a HTTP DELETE request
        /// </summary>
        /// <typeparam name="TReturnType">Type that will be converted from json</typeparam>
        /// <param name="url">Target URL</param>
        /// <returns>Type that will be converted from json</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="HTTPRequestNonSuccessException"></exception>
        Task DELETEAsync<TReturnType>(string url);
    }
}
