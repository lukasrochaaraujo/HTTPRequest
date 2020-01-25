using Newtonsoft.Json;

using System.Collections.Generic;

namespace HTTPRequest.Models
{
    public class HttpResponseErrorModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("errors")]
        public Dictionary<string, object> ErrorsDictionary { get; set; }

        [JsonProperty("exception")]
        public string Exception { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("traceId")]
        public string TraceId { get; set; }
    }
}
