using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ElasticSentinel.Presentation.API.Endpoints;

/// <summary>
/// Elasticsearch Configuration API endpoints for managing Elasticsearch cluster settings
/// </summary>
public static class ElasticConfigurationsEndpoints
{
    public static RouteGroupBuilder MapElasticConfigurationsEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllConfigurations)
            .WithName("GetAllElasticConfigurations")
            .WithDescription("Retrieve all Elasticsearch configurations")
            .Produces<List<ElasticConfiguration>>(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", GetConfigurationById)
            .WithName("GetElasticConfigurationById")
            .WithDescription("Retrieve a specific Elasticsearch configuration by ID")
            .Produces<ElasticConfiguration>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateConfiguration)
            .WithName("CreateElasticConfiguration")
            .WithDescription("Create a new Elasticsearch configuration")
            .Produces<ElasticConfiguration>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", UpdateConfiguration)
            .WithName("UpdateElasticConfiguration")
            .WithDescription("Update an existing Elasticsearch configuration")
            .Produces<ElasticConfiguration>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", DeleteConfiguration)
            .WithName("DeleteElasticConfiguration")
            .WithDescription("Delete an Elasticsearch configuration")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return group;
    }

    private static async Task<IResult> GetAllConfigurations(SentinelDbContext context, CancellationToken ct)
    {
        var configurations = await context.ElasticConfigurations
            .ToListAsync(ct);
        return Results.Ok(configurations);
    }

    private static async Task<IResult> GetConfigurationById(int id, SentinelDbContext context, CancellationToken ct)
    {
        var configuration = await context.ElasticConfigurations
            .FirstOrDefaultAsync(c => c.ElasticConfigId == id, ct);

        return configuration is null
            ? Results.NotFound(new { Message = $"Elasticsearch configuration with ID {id} not found." })
            : Results.Ok(configuration);
    }

    private static async Task<IResult> CreateConfiguration(
        ElasticConfiguration configuration,
        SentinelDbContext context,
        CancellationToken ct)
    {
        context.ElasticConfigurations.Add(configuration);
        await context.SaveChangesAsync(ct);

        return Results.Created($"/api/elastic-configurations/{configuration.ElasticConfigId}", configuration);
    }

    private static async Task<IResult> UpdateConfiguration(
        int id,
        ElasticConfiguration updatedConfiguration,
        SentinelDbContext context,
        CancellationToken ct)
    {
        var configuration = await context.ElasticConfigurations
            .FirstOrDefaultAsync(c => c.ElasticConfigId == id, ct);

        if (configuration is null)
        {
            return Results.NotFound(new { Message = $"Elasticsearch configuration with ID {id} not found." });
        }

        // Update properties
        configuration.ClusterName = updatedConfiguration.ClusterName;
        configuration.ElasticHost = updatedConfiguration.ElasticHost;
        configuration.UserName = updatedConfiguration.UserName;
        configuration.Password = updatedConfiguration.Password;
        configuration.CertificateThumbprint = updatedConfiguration.CertificateThumbprint;
        configuration.IsEnabled = updatedConfiguration.IsEnabled;

        await context.SaveChangesAsync(ct);

        return Results.Ok(configuration);
    }

    private static async Task<IResult> DeleteConfiguration(
        int id,
        SentinelDbContext context,
        CancellationToken ct)
    {
        var configuration = await context.ElasticConfigurations
            .FirstOrDefaultAsync(c => c.ElasticConfigId == id, ct);

        if (configuration is null)
        {
            return Results.NotFound(new { Message = $"Elasticsearch configuration with ID {id} not found." });
        }

        context.ElasticConfigurations.Remove(configuration);
        await context.SaveChangesAsync(ct);

        return Results.NoContent();
    }
}
