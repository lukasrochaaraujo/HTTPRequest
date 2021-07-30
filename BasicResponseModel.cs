namespace HTTPRequest
{
    /// <summary>
    /// Simple class for non complex requests
    /// </summary>
    public class BasicResponseModel
    {
        /// <summary>
        /// HTTP response status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// The response returned by server
        /// </summary>
        public string ResponseBody { get; set; }
    }
}
