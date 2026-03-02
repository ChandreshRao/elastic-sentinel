using System.Text.Json.Serialization;

namespace ElasticSentinel.Application.Common.Models
{
    public class TMBaseIndex
    {
        [JsonPropertyName("server")]
        public string? ServerName { get; set; }

        [JsonPropertyName("@timestamp")]
        public DateTime? LogDtTm { get; set; }

        [JsonPropertyName("level")]
        public string? LogLevel { get; set; }

        [JsonPropertyName("type")]
        public string? LogFileType { get; set; }

        [JsonPropertyName("message")]
        public string? LogMessage { get; set; }

    }
}
