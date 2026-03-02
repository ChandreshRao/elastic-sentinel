using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.Json.Serialization;

namespace ElasticSentinel.Application.Common.Models
{
    public class ErrorLogIndex : TMBaseIndex
    {
        [JsonPropertyName("msgId")]
        public string? MessageId { get; set; }

        [JsonPropertyName("date")]
        public DateTime? ErrorDtTm { get; set; }

        [JsonPropertyName("correlationId")]
        public string? CorrelationId { get; set; }

        [JsonPropertyName("ffaId")]
        public string? ErrorFFAId { get; set; }

        public string? KibanaDiscoverUrl { get; set; }

        private string[]? _PropertiesToConsider = default;

        public ErrorLogIndex GetSerializableObj(string[] PropertiesToConsider)
        {
            _PropertiesToConsider = PropertiesToConsider;
            return this;
        }

        public bool ShouldSerializeKibanaDiscoverUrl()
        {
            return _PropertiesToConsider?.Contains("KibanaDiscoverUrl") ?? true;
        }

        public bool ShouldSerializeCorrelationId()
        {
            return _PropertiesToConsider?.Contains("CorrelationId") ?? true;
        }

        public bool ShouldSerializeErrorFFAId()
        {
            return _PropertiesToConsider?.Contains("ErrorFFAId") ?? true;
        }

        public bool ShouldSerializeMessageId()
        {
            return _PropertiesToConsider?.Contains("MessageId") ?? true;
        }

        public bool ShouldSerializeErrorDtTm()
        {
            return _PropertiesToConsider?.Contains("ErrorDtTm") ?? true;
        }
        public bool ShouldSerializeServerName()
        {
            return _PropertiesToConsider?.Contains("ServerName") ?? true;
        }
        public bool ShouldSerializeLogDtTm()
        {
            return _PropertiesToConsider?.Contains("LogDtTm") ?? true;
        }
        public bool ShouldSerializeLogLevel()
        {
            return _PropertiesToConsider?.Contains("LogLevel") ?? true;
        }
        public bool ShouldSerializeLogFileType()
        {
            return _PropertiesToConsider?.Contains("LogFileType") ?? true;
        }
        public bool ShouldSerializeLogMessage()
        {
            return _PropertiesToConsider?.Contains("LogMessage") ?? true;
        }

    }
}
