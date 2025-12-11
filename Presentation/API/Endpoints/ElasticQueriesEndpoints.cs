using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ElasticSentinel.Presentation.API.Endpoints;

/// <summary>
/// Elastic Query API endpoints for managing Elasticsearch query configurations
/// </summary>
public static class ElasticQueriesEndpoints
{
    public static RouteGroupBuilder MapElasticQueriesEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllQueries)
            .WithName("GetAllElasticQueries")
            .WithDescription("Retrieve all Elasticsearch query configurations")
            .Produces<List<ElasticQuery>>(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", GetQueryById)
            .WithName("GetElasticQueryById")
            .WithDescription("Retrieve a specific Elasticsearch query by ID")
            .Produces<ElasticQuery>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateQuery)
            .WithName("CreateElasticQuery")
            .WithDescription("Create a new Elasticsearch query configuration")
            .Produces<ElasticQuery>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", UpdateQuery)
            .WithName("UpdateElasticQuery")
            .WithDescription("Update an existing Elasticsearch query configuration")
            .Produces<ElasticQuery>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", DeleteQuery)
            .WithName("DeleteElasticQuery")
            .WithDescription("Delete an Elasticsearch query configuration")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return group;
    }

    private static async Task<IResult> GetAllQueries(SentinelDbContext context, CancellationToken ct)
    {
        var queries = await context.ElasticQueries
            .Include(q => q.DynamicRequestDetail)
            .Include(q => q.DynamicResponseDetail)
            .ToListAsync(ct);

        return Results.Ok(queries);
    }

    private static async Task<IResult> GetQueryById(int id, SentinelDbContext context, CancellationToken ct)
    {
        if (id <= 0 || id > short.MaxValue)
        {
            return Results.NotFound(new { Message = $"Query with ID {id} not found" });
        }

        var queryId = (short)id;
        var query = await context.ElasticQueries
            .Include(q => q.DynamicRequestDetail)
            .Include(q => q.DynamicResponseDetail)
            .FirstOrDefaultAsync(q => q.ElasticQueryId == queryId, ct);

        return query is not null
            ? Results.Ok(query)
            : Results.NotFound(new { Message = $"Query with ID {id} not found" });
    }

    private static async Task<IResult> CreateQuery(
        ElasticQuery query,
        SentinelDbContext context,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(query.QueryName))
        {
            return Results.BadRequest(new { Message = "Query name is required" });
        }

        context.ElasticQueries.Add(query);
        await context.SaveChangesAsync(ct);

        return Results.Created($"/api/queries/{query.ElasticQueryId}", query);
    }

    private static async Task<IResult> UpdateQuery(
        int id,
        ElasticQuery updatedQuery,
        SentinelDbContext context,
        CancellationToken ct)
    {
        if (id <= 0 || id > short.MaxValue)
        {
            return Results.NotFound(new { Message = $"Query with ID {id} not found" });
        }

        var queryId = (short)id;
        var query = await context.ElasticQueries.FindAsync([queryId], ct);

        if (query is null)
        {
            return Results.NotFound(new { Message = $"Query with ID {id} not found" });
        }

        if (string.IsNullOrWhiteSpace(updatedQuery.QueryName))
        {
            return Results.BadRequest(new { Message = "Query name is required" });
        }

        query.QueryName = updatedQuery.QueryName;
        query.QueryDescription = updatedQuery.QueryDescription;
        query.IsDynamic = updatedQuery.IsDynamic;
        query.ElasticDynamicQueryDetailId = updatedQuery.ElasticDynamicQueryDetailId;
        query.ElasticDynamicQueryResponseDetailId = updatedQuery.ElasticDynamicQueryResponseDetailId;

        await context.SaveChangesAsync(ct);

        return Results.Ok(query);
    }

    private static async Task<IResult> DeleteQuery(int id, SentinelDbContext context, CancellationToken ct)
    {
        if (id <= 0 || id > short.MaxValue)
        {
            return Results.NotFound(new { Message = $"Query with ID {id} not found" });
        }

        var queryId = (short)id;
        var query = await context.ElasticQueries.FindAsync([queryId], ct);

        if (query is null)
        {
            return Results.NotFound(new { Message = $"Query with ID {id} not found" });
        }

        context.ElasticQueries.Remove(query);
        await context.SaveChangesAsync(ct);

        return Results.NoContent();
    }
}
