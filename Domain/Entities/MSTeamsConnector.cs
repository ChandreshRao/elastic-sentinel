using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticSentinel.Domain.Entities
{
    [Table("teams_connector")]
    public class MSTeamsConnector
    {
        [Key, Column("teams_connector_id")]
        public short MSTeamsConnectorId { get; set; }

        [MaxLength(100), Required, DisplayName("Teams Connector Name"), Column("teams_connector_name")]
        public string Name { get; set; } = default!;

        [MaxLength(2000), Required, DisplayName("Webhook url"), Column("webhook_url")]
        public string WebhookUrl { get; set; } = default!;

        [DisplayName("Enabled"), Column("is_enabled")]
        public bool IsEnabled { get; set; } = true;

        public virtual ICollection<AlertSchedulerConfig>? AlertSchedulerConfigs { get; set; }
    }
}
