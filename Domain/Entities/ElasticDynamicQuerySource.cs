using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ElasticSentinel.Application.Common.Behaviors;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace ElasticSentinel.Domain.Entities
{
    [Table("elastic_dynamic_query_source")]
    public class ElasticDynamicQuerySource
    {
        [Key, Column("elastic_dynamic_query_source_id")]
        public short ElasticDynamicQuerySourceId { get; set; }

        [Required, MaxLength(100), DisplayName("Source name"), Column("source_name")]
        public string SourceName { get; set; } = default!;

        private string _SourceQuery = default!;

        [MaxLength(4000), Required, DisplayName("Source (Query)"), Column("source_query"), JsonCheck(ErrorMessage = "Not a valid json")]
        public string SourceQuery
        {
            get
            {
                return JObject.Parse(_SourceQuery).ToString(Formatting.Indented); 
            }
            set
            {
                _SourceQuery = JObject.Parse(value).ToString(Formatting.None);
            }
        }

        [MaxLength(250), Required, DisplayName("Source Type"), Column("source_type")]
        public string SourceType { get; set; } = default!;

        public virtual ICollection<ElasticDynamicQueryRequestDetail>? RequestDetails { get; set; }
    }
}
