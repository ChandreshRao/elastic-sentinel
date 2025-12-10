using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElasticSentinel.Domain.Entities
{
    [Table("elastic_configuration")]
    public class ElasticConfiguration
    {
        [Key, Column("elastic_configuration_id")]
        public short ElasticConfigId { get; set; }

        [MaxLength(100), Required, DisplayName("Cluster Name"), Column("cluster_name")]
        public string ClusterName { get; set; } = default!;

        [MaxLength(100), Required, DisplayName("Host"), Column("host")]
        public string ElasticHost { get; set; } = default!;

        [MaxLength(100), Required, DisplayName("User Name"), Column("user_name")]
        public string UserName { get; set; } = default!;

        [MaxLength(100), Required, DisplayName("Password"), Column("password")]
        public string Password { get; set; } = default!;

        [MaxLength(2000), DisplayName("Certificate Thumbprint"), Column("certificate_thumbprint")]
        public string? CertificateThumbprint { get; set; }

        [DisplayName("Enabled"), Column("is_enabled"), DefaultValue(true)]
        public bool IsEnabled { get; set; } = true;
    }
}
