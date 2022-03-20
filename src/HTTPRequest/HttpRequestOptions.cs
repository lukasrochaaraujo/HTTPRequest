namespace HTTPRequest
{
    /// <summary>
    /// Settings to be provided for the HttpRequest class.
    /// You can configure date format, timeout, retries and time between retries.
    /// </summary>
    public class HttpRequestOptions
    {
        /// <summary>
        /// The format that a DateTime string should be serialized/deserialized.
        /// <para>Default is "yyyy-MM-ddTHH:mm:ss"</para>
        /// </summary>
        public string DateTimeFormat { get; set; } = "yyyy-MM-ddTHH:mm:ss";

        /// <summary>
        /// Gets or sets the seconds to wait before the request times out.
        /// <para>Default is 30 seconds</para>
        /// </summary>
        public int TimeOutInSeconds { get; set; } = 30;

        /// <summary>
        /// Max number of attempts before throws an exception
        /// <para>Default is 3</para>
        /// </summary>
        public int MaxRequestAttempts { get; set; } = 3;

        /// <summary>
        /// Seconds between each attempt
        /// <para>Default is 3 seconds</para>
        /// </summary>
        public int IntevalBetweenAttemptsInSeconds { get; set; } = 3;
    }
}
