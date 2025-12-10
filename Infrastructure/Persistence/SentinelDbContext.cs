using ElasticSentinel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElasticSentinel.Infrastructure.Persistence
{
    public class SentinelDbContext : DbContext
    {

        public SentinelDbContext(DbContextOptions<SentinelDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public DbSet<EmailConnector> EmailConnectors { get; set; }
        public DbSet<EmailConnectorDetail> EmailConnectorDetails { get; set; }
        public DbSet<MSTeamsConnector> MSTeamsConnectors { get; set; }
        public DbSet<ElasticConfiguration> ElasticConfigurations { get; set; }
        public DbSet<ElasticDynamicQuerySource> ElasticDynamicQuerySources { get; set; }
        public DbSet<ElasticDynamicQueryRequestDetail> ElasticDynamicQueryRequestDetails { get; set; }
        public DbSet<ElasticDynamicQueryResponseDetail> ElasticDynamicQueryResponseDetails { get; set; }
        public DbSet<ElasticDynamicQueryResponseStructure> ElasticDynamicQueryResponseStructures { get; set; }
        public DbSet<NotificationTemplate> NotificationTemplateDetails { get; set; }
        public DbSet<ElasticQuery> ElasticQueries { get; set; }
        public DbSet<AlertSchedulerConfig> AlertSchedulerConfigs { get; set; }
        public DbSet<AlertSchedulerDetail> AlertSchedulerDetails { get; set; }
        public DbSet<DocumentsProcessingDetail> DocumentsProcessingDetails { get; set; }
    }
}
