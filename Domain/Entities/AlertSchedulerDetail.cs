using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticSentinel.Domain.Entities
{
    [Table("alert_scheduler_detail")]
    public class AlertSchedulerDetail
    {
        [Key, Column("alert_scheduler_detail_id")]
        public short AlertSchedulerDetailId { get; set; }

        [Column("alert_scheduler_config_id")]
        public short AlertSchedulerConfigId { get; set; }

        [ForeignKey("AlertSchedulerConfigId")]
        public virtual AlertSchedulerConfig? SchedulerConfig { get; set; }

        [Column("query_filter_dttm") ,DisplayName("Query Filter Timestamp")]
        public DateTime? QueryFilterDtTm { get; set; }

        [Column("last_run_dttm"), DisplayName("Last Run Timestamp")]
        public DateTime? LastRunDtTm { get; set; }

        [Column("last_run_status"), DisplayName("Last Run Status")]
        public string? LastRunStatus { get; set; }

    }
}
