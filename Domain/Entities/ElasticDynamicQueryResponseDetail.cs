using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticSentinel.Domain.Entities
{
    [Table("elastic_dynamic_query_response_detail")]
    public class ElasticDynamicQueryResponseDetail
    {
        [Key, Column("elastic_dynamic_query_response_detail_id")]
        public short ElasticDynamicQueryResponseDetailId { get; set; }

        [Required, Column("query_response_mapper_name"), DisplayName("Query Response Mapper Name")]
        public string QueryResponseMapperName { get; set; } = default!;

        public virtual ICollection<ElasticDynamicQueryResponseStructure>? QueryResponseStructures { get; set; }

        public virtual ICollection<ElasticQuery>? ElasticQueries { get; set; }
    }
}
