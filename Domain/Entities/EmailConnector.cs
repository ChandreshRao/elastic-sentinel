using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticSentinel.Domain.Entities
{
    [Table("email_connector")]
    public class EmailConnector
    {
        [Key, Column("email_connector_id")]
        public short EmailConnectorId { get; set; }

        [MaxLength(100), Required, DisplayName("Email Connector Name"), Column("email_connector_name")]
        public string Name { get; set; } = default!;

        [MaxLength(250), Required, DisplayName("From mail"), Column("from_email")]
        public string FromEmail { get; set; } = default!;

        [MaxLength(100), Required, DisplayName("SMTP Server"), Column("smtp_server")]
        public string PrimarySMTPServer { get; set; } = default!;

        [MaxLength(100), DisplayName("Alternative SMTP Server"), Column("alternate_smtp_server")]
        public string? AlternateSMTPServer { get; set; }

        [Required, DisplayName("SMTP Port"), Column("smtp_port")]
        public int SMTPPort { get; set; }

        [MaxLength(100), DisplayName("User name"), Column("user_name")]
        public string? Username { get; set; }

        [MaxLength(100), DisplayName("Password"), Column("password")]
        public string? Password { get; set; }

        [DisplayName("Enabled"), DefaultValue(true), Column("is_enabled")]
        public bool IsEnabled { get; set; } = true;

        public virtual ICollection<AlertSchedulerConfig>? AlertSchedulerConfigs { get; set; }
    }
}
