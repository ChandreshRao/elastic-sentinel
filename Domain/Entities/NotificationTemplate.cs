using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticSentinel.Domain.Entities
{
    [Table("notification_template")]
    public class NotificationTemplate
    {
        [Key,Column("notification_template_id")]
        public short NotificationTemplateId { get; set; }

        [Required, DisplayName("Template Name"), MaxLength(100), Column("template_name")]
        public string TemplateName { get; set; } = default!;

        [Required, DisplayName("Template Content"), MaxLength(4000), Column("template_content")]
        public string TemplateContent { get; set; } = default!;

        [DefaultValue(true), DisplayName("Enabled"), Column("is_enabled")]
        public bool IsEnabled { get; set; }

        public virtual ICollection<AlertSchedulerConfig>? AlertSchedulerConfigs { get; set; }
    }
}
