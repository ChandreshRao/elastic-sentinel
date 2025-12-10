using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticSentinel.Domain.Entities
{
    [Table("elastic_query")]
    public class ElasticQuery
    {
        [Key, Column("elastic_query_id")]
        public short ElasticQueryId { get; set; }

        [MaxLength(100), Required, Column("query_name")]
        public string QueryName { get; set; } = default!;

        [MaxLength(1000), Required, Column("query_description")]
        public string? QueryDescription { get; set; }

        [DisplayName("Is Dynamic"), Column("is_dynamic"), DefaultValue(false)]
        public bool IsDynamic { get; set; } = false;

        [Column("elastic_dynamic_query_request_detail_id")]
        public short ElasticDynamicQueryDetailId { get; set; }

        [Column("elastic_dynamic_query_response_detail_id")]
        public short ElasticDynamicQueryResponseDetailId { get; set; }

        [ForeignKey("ElasticDynamicQueryDetailId")]
        public virtual ElasticDynamicQueryRequestDetail? DynamicRequestDetail { get; set; }

        [ForeignKey("ElasticDynamicQueryResponseDetailId")]
        public virtual ElasticDynamicQueryResponseDetail? DynamicResponseDetail { get; set; }

        public virtual ICollection<AlertSchedulerConfig>? AlertSchedulerConfigs { get; set; }

    }
}
