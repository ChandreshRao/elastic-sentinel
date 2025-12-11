using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ElasticSentinel.Presentation.API.Endpoints;

/// <summary>
/// Connector API endpoints for managing email and Teams notification connectors
/// </summary>
public static class ConnectorsEndpoints
{
    public static RouteGroupBuilder MapConnectorsEndpoints(this RouteGroupBuilder group)
    {
        // Email Connector routes
        var emailGroup = group.MapGroup("/email");
        
        emailGroup.MapGet("/", GetAllEmailConnectors)
            .WithName("GetAllEmailConnectors")
            .WithDescription("Retrieve all email connector configurations")
            .Produces<List<EmailConnector>>(StatusCodes.Status200OK);

        emailGroup.MapGet("/{id:int}", GetEmailConnectorById)
            .WithName("GetEmailConnectorById")
            .Produces<EmailConnector>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        emailGroup.MapPost("/", CreateEmailConnector)
            .WithName("CreateEmailConnector")
            .Produces<EmailConnector>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        emailGroup.MapPut("/{id:int}", UpdateEmailConnector)
            .WithName("UpdateEmailConnector")
            .Produces<EmailConnector>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        emailGroup.MapDelete("/{id:int}", DeleteEmailConnector)
            .WithName("DeleteEmailConnector")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        // Email Connector Details (Recipients) routes
        var emailDetailsGroup = group.MapGroup("/email-details");
        
        emailDetailsGroup.MapGet("/", GetAllEmailConnectorDetails)
            .WithName("GetAllEmailConnectorDetails")
            .Produces<List<EmailConnectorDetail>>(StatusCodes.Status200OK);

        emailDetailsGroup.MapGet("/{id:int}", GetEmailConnectorDetailById)
            .WithName("GetEmailConnectorDetailById")
            .Produces<EmailConnectorDetail>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        emailDetailsGroup.MapPost("/", CreateEmailConnectorDetail)
            .WithName("CreateEmailConnectorDetail")
            .Produces<EmailConnectorDetail>(StatusCodes.Status201Created);

        emailDetailsGroup.MapPut("/{id:int}", UpdateEmailConnectorDetail)
            .WithName("UpdateEmailConnectorDetail")
            .Produces<EmailConnectorDetail>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        emailDetailsGroup.MapDelete("/{id:int}", DeleteEmailConnectorDetail)
            .WithName("DeleteEmailConnectorDetail")
            .Produces(StatusCodes.Status204NoContent);

        // Teams Connector routes
        var teamsGroup = group.MapGroup("/teams");
        
        teamsGroup.MapGet("/", GetAllTeamsConnectors)
            .WithName("GetAllTeamsConnectors")
            .Produces<List<MSTeamsConnector>>(StatusCodes.Status200OK);

        teamsGroup.MapGet("/{id:int}", GetTeamsConnectorById)
            .WithName("GetTeamsConnectorById")
            .Produces<MSTeamsConnector>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        teamsGroup.MapPost("/", CreateTeamsConnector)
            .WithName("CreateTeamsConnector")
            .Produces<MSTeamsConnector>(StatusCodes.Status201Created);

        teamsGroup.MapPut("/{id:int}", UpdateTeamsConnector)
            .WithName("UpdateTeamsConnector")
            .Produces<MSTeamsConnector>(StatusCodes.Status200OK);

        teamsGroup.MapDelete("/{id:int}", DeleteTeamsConnector)
            .WithName("DeleteTeamsConnector")
            .Produces(StatusCodes.Status204NoContent);

        return group;
    }

    #region Email Connectors

    private static async Task<IResult> GetAllEmailConnectors(SentinelDbContext context, CancellationToken ct)
    {
        var connectors = await context.EmailConnectors.ToListAsync(ct);
        return Results.Ok(connectors);
    }

    private static async Task<IResult> GetEmailConnectorById(short id, SentinelDbContext context, CancellationToken ct)
    {
        var connector = await context.EmailConnectors.FindAsync([id], ct);
        return connector is not null
            ? Results.Ok(connector)
            : Results.NotFound(new { Message = $"Email connector with ID {id} not found" });
    }

    private static async Task<IResult> CreateEmailConnector(
        EmailConnector connector,
        SentinelDbContext context,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(connector.Name) || string.IsNullOrWhiteSpace(connector.FromEmail))
        {
            return Results.BadRequest(new { Message = "Name and From Email are required" });
        }

        context.EmailConnectors.Add(connector);
        await context.SaveChangesAsync(ct);

        return Results.Created($"/api/connectors/email/{connector.EmailConnectorId}", connector);
    }

    private static async Task<IResult> UpdateEmailConnector(
        short id,
        EmailConnector updatedConnector,
        SentinelDbContext context,
        CancellationToken ct)
    {
        var connector = await context.EmailConnectors.FindAsync([id], ct);

        if (connector is null)
        {
            return Results.NotFound(new { Message = $"Email connector with ID {id} not found" });
        }

        connector.Name = updatedConnector.Name;
        connector.FromEmail = updatedConnector.FromEmail;
        connector.PrimarySMTPServer = updatedConnector.PrimarySMTPServer;
        connector.AlternateSMTPServer = updatedConnector.AlternateSMTPServer;
        connector.SMTPPort = updatedConnector.SMTPPort;
        connector.Username = updatedConnector.Username;
        connector.Password = updatedConnector.Password;
        connector.IsEnabled = updatedConnector.IsEnabled;

        await context.SaveChangesAsync(ct);

        return Results.Ok(connector);
    }

    private static async Task<IResult> DeleteEmailConnector(short id, SentinelDbContext context, CancellationToken ct)
    {
        var connector = await context.EmailConnectors.FindAsync([id], ct);

        if (connector is null)
        {
            return Results.NotFound(new { Message = $"Email connector with ID {id} not found" });
        }

        context.EmailConnectors.Remove(connector);
        await context.SaveChangesAsync(ct);

        return Results.NoContent();
    }

    #endregion

    #region Email Connector Details

    private static async Task<IResult> GetAllEmailConnectorDetails(SentinelDbContext context, CancellationToken ct)
    {
        var details = await context.EmailConnectorDetails.ToListAsync(ct);
        return Results.Ok(details);
    }

    private static async Task<IResult> GetEmailConnectorDetailById(short id, SentinelDbContext context, CancellationToken ct)
    {
        var detail = await context.EmailConnectorDetails.FindAsync([id], ct);
        return detail is not null
            ? Results.Ok(detail)
            : Results.NotFound(new { Message = $"Email connector detail with ID {id} not found" });
    }

    private static async Task<IResult> CreateEmailConnectorDetail(
        EmailConnectorDetail detail,
        SentinelDbContext context,
        CancellationToken ct)
    {
        context.EmailConnectorDetails.Add(detail);
        await context.SaveChangesAsync(ct);

        return Results.Created($"/api/connectors/email-details/{detail.EmailAlertDetailId}", detail);
    }

    private static async Task<IResult> UpdateEmailConnectorDetail(
        short id,
        EmailConnectorDetail updatedDetail,
        SentinelDbContext context,
        CancellationToken ct)
    {
        var detail = await context.EmailConnectorDetails.FindAsync([id], ct);

        if (detail is null)
        {
            return Results.NotFound(new { Message = $"Email connector detail with ID {id} not found" });
        }

        detail.Name = updatedDetail.Name;
        detail.EmailSubject = updatedDetail.EmailSubject;
        detail.ToEmails = updatedDetail.ToEmails;
        detail.CcEmails = updatedDetail.CcEmails;

        await context.SaveChangesAsync(ct);

        return Results.Ok(detail);
    }

    private static async Task<IResult> DeleteEmailConnectorDetail(short id, SentinelDbContext context, CancellationToken ct)
    {
        var detail = await context.EmailConnectorDetails.FindAsync([id], ct);

        if (detail is null)
        {
            return Results.NotFound(new { Message = $"Email connector detail with ID {id} not found" });
        }

        context.EmailConnectorDetails.Remove(detail);
        await context.SaveChangesAsync(ct);

        return Results.NoContent();
    }

    #endregion

    #region Teams Connectors

    private static async Task<IResult> GetAllTeamsConnectors(SentinelDbContext context, CancellationToken ct)
    {
        var connectors = await context.MSTeamsConnectors.ToListAsync(ct);
        return Results.Ok(connectors);
    }

    private static async Task<IResult> GetTeamsConnectorById(short id, SentinelDbContext context, CancellationToken ct)
    {
        var connector = await context.MSTeamsConnectors.FindAsync([id], ct);
        return connector is not null
            ? Results.Ok(connector)
            : Results.NotFound(new { Message = $"Teams connector with ID {id} not found" });
    }

    private static async Task<IResult> CreateTeamsConnector(
        MSTeamsConnector connector,
        SentinelDbContext context,
        CancellationToken ct)
    {
        context.MSTeamsConnectors.Add(connector);
        await context.SaveChangesAsync(ct);

        return Results.Created($"/api/connectors/teams/{connector.MSTeamsConnectorId}", connector);
    }

    private static async Task<IResult> UpdateTeamsConnector(
        short id,
        MSTeamsConnector updatedConnector,
        SentinelDbContext context,
        CancellationToken ct)
    {
        var connector = await context.MSTeamsConnectors.FindAsync([id], ct);

        if (connector is null)
        {
            return Results.NotFound(new { Message = $"Teams connector with ID {id} not found" });
        }

        connector.Name = updatedConnector.Name;
        connector.WebhookUrl = updatedConnector.WebhookUrl;
        connector.IsEnabled = updatedConnector.IsEnabled;

        await context.SaveChangesAsync(ct);

        return Results.Ok(connector);
    }

    private static async Task<IResult> DeleteTeamsConnector(short id, SentinelDbContext context, CancellationToken ct)
    {
        var connector = await context.MSTeamsConnectors.FindAsync([id], ct);

        if (connector is null)
        {
            return Results.NotFound(new { Message = $"Teams connector with ID {id} not found" });
        }

        context.MSTeamsConnectors.Remove(connector);
        await context.SaveChangesAsync(ct);

        return Results.NoContent();
    }

    #endregion
}
