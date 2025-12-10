using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticSentinel.Domain.Entities
{
    [Table("elastic_dynamic_query_response_structure")]
    public class ElasticDynamicQueryResponseStructure
    {
        [Key, Column("elastic_dynamic_query_response_structure_id")]
        public int ElasticDynamicQueryResponseStructureId { get; set; }

        [Column("elastic_dynamic_query_response_detail_id"), DisplayName("Query Response Mapper Name")]
        public short ElasticDynamicQueryResponseDetailId { get; set; }

        [Column("is_index_field_array"), DisplayName("Is Index field an Array?"), DefaultValue(false)]
        public bool IsIndexFieldAnArray { get; set; }

        [Column("index_root_field_name"), DisplayName("Index root field name")]
        public string? IndexRootFieldName { get; set; }

        [Column("index_field_name"), DisplayName("Index field name"), Required]
        public string IndexFieldName { get; set; } = default!;

        [Column("alias_field_name"), DisplayName("Alias field name"), Required]
        public string AliasFieldName { get; set; } = default!;

        [ForeignKey("ElasticDynamicQueryResponseDetailId"), DisplayName("Query Response Mapper Name")]
        public virtual ElasticDynamicQueryResponseDetail? DynamicQueryResponseDetail { get; set; }
    }
}
