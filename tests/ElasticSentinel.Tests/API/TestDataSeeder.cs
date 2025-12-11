using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Persistence;

namespace ElasticSentinel.Tests.API;

public static class TestDataSeeder
{
    public static void SeedTestData(SentinelDbContext context)
    {
        // Ensure database is created with schema
        context.Database.EnsureCreated();

        // Seed Elastic Configurations
        var elasticConfig = new ElasticConfiguration
        {
            ElasticConfigId = 1,
            ClusterName = "Test Cluster",
            ElasticHost = "https://localhost:9200",
            UserName = "elastic",
            Password = "password",
            IsEnabled = true
        };
        context.ElasticConfigurations.Add(elasticConfig);

        // Seed ElasticDynamicQuerySource (required by ElasticDynamicQueryRequestDetail)
        var querySource = new ElasticDynamicQuerySource
        {
            ElasticDynamicQuerySourceId = 1,
            SourceName = "Test Source",
            SourceQuery = "{\"query\":{\"match_all\":{}}}",
            SourceType = "Elasticsearch"
        };
        context.ElasticDynamicQuerySources.Add(querySource);

        // Seed Dynamic Query Request and Response Details
        var queryRequestDetail = new ElasticDynamicQueryRequestDetail
        {
            ElasticDynamicQueryDetailId = 1,
            DyanmicQueryName = "Test Request Detail",
            HTTPMethod = "GET",
            QueryType = "_search",
            ElasticDynamicQuerySourceId = 1
        };
        context.ElasticDynamicQueryRequestDetails.Add(queryRequestDetail);

        var queryResponseDetail = new ElasticDynamicQueryResponseDetail
        {
            ElasticDynamicQueryResponseDetailId = 1,
            QueryResponseMapperName = "Test Response Detail"
        };
        context.ElasticDynamicQueryResponseDetails.Add(queryResponseDetail);

        // Seed Elastic Query
        var elasticQuery = new ElasticQuery
        {
            ElasticQueryId = 1,
            QueryName = "Test Query",
            QueryDescription = "Test Description",
            IsDynamic = false,
            ElasticDynamicQueryDetailId = 1,
            ElasticDynamicQueryResponseDetailId = 1
        };
        context.ElasticQueries.Add(elasticQuery);

        // Seed Email Connector
        var emailConnector = new EmailConnector
        {
            EmailConnectorId = 1,
            Name = "Test Email Connector",
            FromEmail = "test@test.com",
            PrimarySMTPServer = "smtp.test.com",
            SMTPPort = 587,
            Username = "test",
            Password = "password",
            IsEnabled = true
        };
        context.EmailConnectors.Add(emailConnector);

        // Seed MS Teams Connector
        var teamsConnector = new MSTeamsConnector
        {
            MSTeamsConnectorId = 1,
            Name = "Test Teams Connector",
            WebhookUrl = "https://teams.webhook.url",
            IsEnabled = true
        };
        context.MSTeamsConnectors.Add(teamsConnector);

        // Seed Email Connector Detail
        var emailConnectorDetail = new EmailConnectorDetail
        {
            EmailAlertDetailId = 1,
            Name = "Test Email Detail",
            ToEmails = "recipient@test.com",
            EmailSubject = "Test Subject",
            CcEmails = "cc@test.com"
        };
        context.EmailConnectorDetails.Add(emailConnectorDetail);

        // Seed Notification Template
        var notificationTemplate = new NotificationTemplate
        {
            NotificationTemplateId = 1,
            TemplateName = "Test Template",
            TemplateContent = "Test Content",
            IsEnabled = true
        };
        context.NotificationTemplateDetails.Add(notificationTemplate);

        // Save all seed data
        context.SaveChanges();
    }

    public static void CleanupTestData(SentinelDbContext context)
    {
        // Clear all data from the database
        context.AlertSchedulerConfigs.RemoveRange(context.AlertSchedulerConfigs);
        context.AlertSchedulerDetails.RemoveRange(context.AlertSchedulerDetails);
        context.NotificationTemplateDetails.RemoveRange(context.NotificationTemplateDetails);
        context.EmailConnectorDetails.RemoveRange(context.EmailConnectorDetails);
        context.MSTeamsConnectors.RemoveRange(context.MSTeamsConnectors);
        context.EmailConnectors.RemoveRange(context.EmailConnectors);
        context.ElasticQueries.RemoveRange(context.ElasticQueries);
        context.ElasticDynamicQueryResponseDetails.RemoveRange(context.ElasticDynamicQueryResponseDetails);
        context.ElasticDynamicQueryRequestDetails.RemoveRange(context.ElasticDynamicQueryRequestDetails);
        context.ElasticDynamicQuerySources.RemoveRange(context.ElasticDynamicQuerySources);
        context.ElasticConfigurations.RemoveRange(context.ElasticConfigurations);
        context.SaveChanges();
    }
}
