using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticSentinel.Domain.Entities
{
    [Table("email_connector_detail")]
    public class EmailConnectorDetail
    {
        [Key, Column("email_connector_detail_id")]
        public short EmailAlertDetailId { get; set; }

        [MaxLength(100), DisplayName("Alert Detail Name"), Column("email_connector_detail_name")]
        public string Name { get; set; } = default!;

        [MaxLength(100), DisplayName("Email Subject"), Column("email_subject")]
        public string? EmailSubject { get; set; }

        [Required, MaxLength(1000), DisplayName("To Emails (',' seperated)"), Column("to_emails")]
        public string ToEmails { get; set; } = default!;

        [MaxLength(1000), DisplayName("Cc Emails (',' seperated)"), Column("cc_emails")]
        public string? CcEmails { get; set; }

        public virtual ICollection<AlertSchedulerConfig>? AlertSchedulerConfigs { get; set; }
    }
}
