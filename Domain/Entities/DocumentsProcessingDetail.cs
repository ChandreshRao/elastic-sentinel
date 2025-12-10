using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticSentinel.Domain.Entities
{
    [Table("documents_processing_detail")]
    public class DocumentsProcessingDetail
    {
        [Column("documents_processing_detail_id")]
        public int DocumentsProcessingDetailId { get; set; }

        [Column("created_dttm")]
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

        [Column("document_data")]
        public string DocumentData { get; set; } = default!;

        [Column("status"), DefaultValue('P')]
        public char Status { get; set; }

        [Column("is_notified"), DefaultValue(false)]
        public bool IsNotified { get; set; }

        [Column("retry_attempts"), DefaultValue(0)]
        public int RetryAttempts { get; set; }
    }
}
