namespace ElasticSentinel.Application.Common.Models
{
    public class EmailConnectorDetails
    {
        public string EmailSubject { get; set; } = default!;

        public string FromEmail { get; set; } = default!;

        public int SMTPPort { get; set; }

        public string SMTPServer { get; set; } = default!; 

        public string? SMTPAltServer { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string ToEmails { get; set; } = default!; 

        public string? CcEmails { get; set; }
    }
}
