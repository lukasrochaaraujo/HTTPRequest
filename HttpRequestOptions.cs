namespace HTTPRequest
{
    public class HttpRequestOptions
    {
        public string DateTimeFormat { get; set; } = "yyyy-MM-ddTHH:mm:ss";

        public int TimeOutInSeconds { get; set; } = 30;

        public int MaxRequestAttempts { get; set; } = 3;
    }
}
