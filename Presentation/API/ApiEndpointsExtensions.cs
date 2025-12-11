using ElasticSentinel.Presentation.API.Endpoints;

namespace ElasticSentinel.Presentation.API;

/// <summary>
/// Extension methods for registering all API endpoints
/// </summary>
public static class ApiEndpointsExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        var apiGroup = app.MapGroup("/api")
            .WithTags("API")
            .RequireAuthorization();

        // Map all endpoint groups
        apiGroup.MapGroup("/queries")
            .MapElasticQueriesEndpoints()
            .WithTags("Queries");

        apiGroup.MapGroup("/connectors")
            .MapConnectorsEndpoints()
            .WithTags("Connectors");

        apiGroup.MapGroup("/scheduler")
            .MapSchedulerEndpoints()
            .WithTags("Scheduler");

        apiGroup.MapGroup("/templates")
            .MapNotificationTemplatesEndpoints()
            .WithTags("Templates");

        apiGroup.MapGroup("/elastic-configurations")
            .MapElasticConfigurationsEndpoints()
            .WithTags("Elasticsearch Configurations");

        return app;
    }
}
