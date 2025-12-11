using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ElasticSentinel.Presentation.API.Endpoints;

/// <summary>
/// Scheduler API endpoints for managing alert scheduler configurations
/// </summary>
public static class SchedulerEndpoints
{
    public static RouteGroupBuilder MapSchedulerEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/configs", GetAllSchedulerConfigs)
            .WithName("GetAllSchedulerConfigs")
            .WithDescription("Retrieve all scheduler configurations")
            .Produces<List<AlertSchedulerConfig>>(StatusCodes.Status200OK);

        group.MapGet("/configs/{id:int}", GetSchedulerConfigById)
            .WithName("GetSchedulerConfigById")
            .Produces<AlertSchedulerConfig>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/configs", CreateSchedulerConfig)
            .WithName("CreateSchedulerConfig")
            .Produces<AlertSchedulerConfig>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("/configs/{id:int}", UpdateSchedulerConfig)
            .WithName("UpdateSchedulerConfig")
            .Produces<AlertSchedulerConfig>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/configs/{id:int}", DeleteSchedulerConfig)
            .WithName("DeleteSchedulerConfig")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        // Scheduler Details routes
        group.MapGet("/details", GetAllSchedulerDetails)
            .WithName("GetAllSchedulerDetails")
            .Produces<List<AlertSchedulerDetail>>(StatusCodes.Status200OK);

        group.MapGet("/details/{id:int}", GetSchedulerDetailById)
            .WithName("GetSchedulerDetailById")
            .Produces<AlertSchedulerDetail>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/details", CreateSchedulerDetail)
            .WithName("CreateSchedulerDetail")
            .Produces<AlertSchedulerDetail>(StatusCodes.Status201Created);

        group.MapPut("/details/{id:int}", UpdateSchedulerDetail)
            .WithName("UpdateSchedulerDetail")
            .Produces<AlertSchedulerDetail>(StatusCodes.Status200OK);

        group.MapDelete("/details/{id:int}", DeleteSchedulerDetail)
            .WithName("DeleteSchedulerDetail")
            .Produces(StatusCodes.Status204NoContent);

        return group;
    }

    #region Scheduler Configs

    private static async Task<IResult> GetAllSchedulerConfigs(SentinelDbContext context, CancellationToken ct)
    {
        var configs = await context.AlertSchedulerConfigs
            .Include(c => c.Query)
            .ToListAsync(ct);

        return Results.Ok(configs);
    }

    private static async Task<IResult> GetSchedulerConfigById(int id, SentinelDbContext context, CancellationToken ct)
    {
        if (id <= 0 || id > short.MaxValue)
        {
            return Results.NotFound(new { Message = $"Scheduler config with ID {id} not found" });
        }

        var configId = (short)id;
        var config = await context.AlertSchedulerConfigs
            .Include(c => c.Query)
            .FirstOrDefaultAsync(c => c.AlertSchedulerConfigId == configId, ct);

        return config is not null
            ? Results.Ok(config)
            : Results.NotFound(new { Message = $"Scheduler config with ID {id} not found" });
    }

    private static async Task<IResult> CreateSchedulerConfig(
        AlertSchedulerConfig config,
        SentinelDbContext context,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(config.SchedulerName))
        {
            return Results.BadRequest(new { Message = "Scheduler name is required" });
        }

        context.AlertSchedulerConfigs.Add(config);
        await context.SaveChangesAsync(ct);

        return Results.Created($"/api/scheduler/configs/{config.AlertSchedulerConfigId}", config);
    }

    private static async Task<IResult> UpdateSchedulerConfig(
        int id,
        AlertSchedulerConfig updatedConfig,
        SentinelDbContext context,
        CancellationToken ct)
    {
        if (id <= 0 || id > short.MaxValue)
        {
            return Results.NotFound(new { Message = $"Scheduler config with ID {id} not found" });
        }

        var configId = (short)id;
        var config = await context.AlertSchedulerConfigs.FindAsync([configId], ct);

        if (config is null)
        {
            return Results.NotFound(new { Message = $"Scheduler config with ID {id} not found" });
        }

        config.SchedulerName = updatedConfig.SchedulerName;
        config.SchedulerGroup = updatedConfig.SchedulerGroup;
        config.ElasticQueryId = updatedConfig.ElasticQueryId;
        config.IsEnabled = updatedConfig.IsEnabled;
        config.CronExp = updatedConfig.CronExp;

        await context.SaveChangesAsync(ct);

        return Results.Ok(config);
    }

    private static async Task<IResult> DeleteSchedulerConfig(int id, SentinelDbContext context, CancellationToken ct)
    {
        if (id <= 0 || id > short.MaxValue)
        {
            return Results.NotFound(new { Message = $"Scheduler config with ID {id} not found" });
        }

        var configId = (short)id;
        var config = await context.AlertSchedulerConfigs.FindAsync([configId], ct);

        if (config is null)
        {
            return Results.NotFound(new { Message = $"Scheduler config with ID {id} not found" });
        }

        context.AlertSchedulerConfigs.Remove(config);
        await context.SaveChangesAsync(ct);

        return Results.NoContent();
    }

    #endregion

    #region Scheduler Details

    private static async Task<IResult> GetAllSchedulerDetails(SentinelDbContext context, CancellationToken ct)
    {
        var details = await context.AlertSchedulerDetails.ToListAsync(ct);
        return Results.Ok(details);
    }

    private static async Task<IResult> GetSchedulerDetailById(int id, SentinelDbContext context, CancellationToken ct)
    {
        var detail = await context.AlertSchedulerDetails.FindAsync([id], ct);
        return detail is not null
            ? Results.Ok(detail)
            : Results.NotFound(new { Message = $"Scheduler detail with ID {id} not found" });
    }

    private static async Task<IResult> CreateSchedulerDetail(
        AlertSchedulerDetail detail,
        SentinelDbContext context,
        CancellationToken ct)
    {
        context.AlertSchedulerDetails.Add(detail);
        await context.SaveChangesAsync(ct);

        return Results.Created($"/api/scheduler/details/{detail.AlertSchedulerDetailId}", detail);
    }

    private static async Task<IResult> UpdateSchedulerDetail(
        int id,
        AlertSchedulerDetail updatedDetail,
        SentinelDbContext context,
        CancellationToken ct)
    {
        var detail = await context.AlertSchedulerDetails.FindAsync([id], ct);

        if (detail is null)
        {
            return Results.NotFound(new { Message = $"Scheduler detail with ID {id} not found" });
        }

        detail.AlertSchedulerConfigId = updatedDetail.AlertSchedulerConfigId;
        detail.QueryFilterDtTm = updatedDetail.QueryFilterDtTm;
        detail.LastRunDtTm = updatedDetail.LastRunDtTm;
        detail.LastRunStatus = updatedDetail.LastRunStatus;

        await context.SaveChangesAsync(ct);

        return Results.Ok(detail);
    }

    private static async Task<IResult> DeleteSchedulerDetail(int id, SentinelDbContext context, CancellationToken ct)
    {
        var detail = await context.AlertSchedulerDetails.FindAsync([id], ct);

        if (detail is null)
        {
            return Results.NotFound(new { Message = $"Scheduler detail with ID {id} not found" });
        }

        context.AlertSchedulerDetails.Remove(detail);
        await context.SaveChangesAsync(ct);

        return Results.NoContent();
    }

    #endregion
}
