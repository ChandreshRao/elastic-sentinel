using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticSentinel.Domain.Entities
{
    [Table("alert_scheduler_config")]
    public class AlertSchedulerConfig
    {
        [Key, Column("alert_scheduler_config_id")]
        public short AlertSchedulerConfigId { get; set; }

        [Column("elastic_configuration_id")]
        public short ElasticConfigId { get; set; }

        [MaxLength(100), Required, DisplayName("Scheduler Name"), Column("scheduler_name")]
        public string SchedulerName { get; set; } = default!;

        [MaxLength(100), Required, DisplayName("Scheduler Group"), Column("scheduler_group")]
        public string SchedulerGroup { get; set; } = default!;

        [MaxLength(100), Required, DisplayName("Cron Expression"), Column("cron_expression")]
        public string CronExp { get; set; } = default!;

        [Column("elastic_query_id")]
        public short ElasticQueryId { get; set; }

        [Column("email_connector_id")]
        public short EmailConnectorId { get; set; }

        [Column("teams_connector_id")]
        public short MSTeamsConnectorId { get; set; }

        [Column("email_connector_detail_id")]
        public short EmailAlertDetailId { get; set; }

        [Column("notification_template_id")]
        public short NotificationTemplateId { get; set; }

        [DisplayName("Enabled"), Column("is_enabled")]
        public bool IsEnabled { get; set; } = true;

        [DisplayName("Elastic Cluster Name"), ForeignKey("ElasticConfigId")]
        public virtual ElasticConfiguration? ElasticConfig { get; set; }

        [DisplayName("Elastic Query"), ForeignKey("ElasticQueryId")]
        public virtual ElasticQuery? Query { get; set; }

        [DisplayName("Email Connector"), ForeignKey("EmailConnectorId")]
        public virtual EmailConnector? MailConnector { get; set; }

        [DisplayName("Teams Connector"), ForeignKey("MSTeamsConnectorId")]
        public virtual MSTeamsConnector? TeamsConnector { get; set; }

        [DisplayName("Email Alert Detail"), ForeignKey("EmailAlertDetailId")]
        public virtual EmailConnectorDetail? MailConnectorDetail { get; set; }

        [DisplayName("Template"), ForeignKey("NotificationTemplateId")]
        public virtual NotificationTemplate? Template { get; set; }

        public virtual ICollection<AlertSchedulerDetail>? AlertSchedulerDetails { get; set; }
    }
}
