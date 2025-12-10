using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticSentinel.Domain.Entities
{
    [Table("elastic_dynamic_query_request_detail")]
    public class ElasticDynamicQueryRequestDetail
    {
        [Key, Column("elastic_dynamic_query_request_detail_id")]
        public short ElasticDynamicQueryDetailId { get; set; }

        [MaxLength(100), Required, DisplayName("Request Name"), Column("request_name")]
        public string DyanmicQueryName { get; set; } = default!;

        [MaxLength(20), Required, Column("http_method")]
        public string HTTPMethod { get; set; } = "GET";

        [MaxLength(500), DisplayName("Index Name"), Column("index_name")]
        public string? IndexName { get; set; }

        [DisplayName("Is Index name an expression?"), Column("is_index_expression")]
        public bool IsIndexNameExpression { get; set; } = false;

        [MaxLength(500), Required, DisplayName("Query type"), Column("query_type")]
        public string QueryType { get; set; } = "_search";

        [MaxLength(4000), DisplayName("Params"), Column("query_params")]
        public string? QueryParams { get; set; }

        [Column("elastic_dynamic_query_source_id")]
        public short ElasticDynamicQuerySourceId { get; set; }

        [MaxLength(4000), DisplayName("Headers"), Column("headers")]
        public string? Headers { get; set; }

        [MaxLength(50), DisplayName("Auth Type (Basic/Bearer)"), Column("auth_type")]
        public string? AuthType { get; set; } = "Basic";

        [MaxLength(4000), DisplayName("Message Body"), Column("message_body")]
        public string? Body { get; set; }

        [ForeignKey("ElasticDynamicQuerySourceId")]
        public virtual ElasticDynamicQuerySource? QuerySource { get; set; }
    }
}
