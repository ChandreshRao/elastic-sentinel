using ElasticSentinel.Domain.Entities;
using ElasticSentinel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ElasticSentinel.Presentation.API.Endpoints;

/// <summary>
/// Notification Templates API endpoints for managing alert message templates
/// </summary>
public static class NotificationTemplatesEndpoints
{
    public static RouteGroupBuilder MapNotificationTemplatesEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllTemplates)
            .WithName("GetAllNotificationTemplates")
            .WithDescription("Retrieve all notification templates")
            .Produces<List<NotificationTemplate>>(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", GetTemplateById)
            .WithName("GetNotificationTemplateById")
            .Produces<NotificationTemplate>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateTemplate)
            .WithName("CreateNotificationTemplate")
            .Produces<NotificationTemplate>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", UpdateTemplate)
            .WithName("UpdateNotificationTemplate")
            .Produces<NotificationTemplate>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:int}", DeleteTemplate)
            .WithName("DeleteNotificationTemplate")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return group;
    }

    private static async Task<IResult> GetAllTemplates(SentinelDbContext context, CancellationToken ct)
    {
        var templates = await context.NotificationTemplateDetails.ToListAsync(ct);
        return Results.Ok(templates);
    }

    private static async Task<IResult> GetTemplateById(int id, SentinelDbContext context, CancellationToken ct)
    {
        if (id <= 0 || id > short.MaxValue)
        {
            return Results.NotFound(new { Message = $"Notification template with ID {id} not found" });
        }

        var templateId = (short)id;
        var template = await context.NotificationTemplateDetails.FindAsync([templateId], ct);
        return template is not null
            ? Results.Ok(template)
            : Results.NotFound(new { Message = $"Notification template with ID {id} not found" });
    }

    private static async Task<IResult> CreateTemplate(
        NotificationTemplate template,
        SentinelDbContext context,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(template.TemplateName))
        {
            return Results.BadRequest(new { Message = "Template name is required" });
        }

        context.NotificationTemplateDetails.Add(template);
        await context.SaveChangesAsync(ct);

        return Results.Created($"/api/templates/{template.NotificationTemplateId}", template);
    }

    private static async Task<IResult> UpdateTemplate(
        int id,
        NotificationTemplate updatedTemplate,
        SentinelDbContext context,
        CancellationToken ct)
    {
        if (id <= 0 || id > short.MaxValue)
        {
            return Results.NotFound(new { Message = $"Notification template with ID {id} not found" });
        }

        var templateId = (short)id;
        var template = await context.NotificationTemplateDetails.FindAsync([templateId], ct);

        if (template is null)
        {
            return Results.NotFound(new { Message = $"Notification template with ID {id} not found" });
        }

        template.TemplateName = updatedTemplate.TemplateName;
        template.TemplateContent = updatedTemplate.TemplateContent;
        template.IsEnabled = updatedTemplate.IsEnabled;

        await context.SaveChangesAsync(ct);

        return Results.Ok(template);
    }

    private static async Task<IResult> DeleteTemplate(int id, SentinelDbContext context, CancellationToken ct)
    {
        if (id <= 0 || id > short.MaxValue)
        {
            return Results.NotFound(new { Message = $"Notification template with ID {id} not found" });
        }

        var templateId = (short)id;
        var template = await context.NotificationTemplateDetails.FindAsync([templateId], ct);

        if (template is null)
        {
            return Results.NotFound(new { Message = $"Notification template with ID {id} not found" });
        }

        context.NotificationTemplateDetails.Remove(template);
        await context.SaveChangesAsync(ct);

        return Results.NoContent();
    }
}
