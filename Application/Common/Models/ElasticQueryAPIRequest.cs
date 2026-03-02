namespace ElasticSentinel.Application.Common.Models
{
    public class ElasticQueryAPIRequest
    {
        public string ElasticHost { get; set; } = default!;

        public string UserName { get; set; } = default!;

        public string Password { get; set; } = default!;

        public string AuthType { get; set; } = default!;

        public string? QueryName { get; set; }

        public List<string>? QuerySuffixes { get; set; }

        public Dictionary<string, string?>? QueryParams { get; set; }

        public Dictionary<string, string>? Headers { get; set; }

        public string? RequestBody { get; set; }
    }
}
