using ElasticSentinel.Application.Common.Abstractions;
using ElasticSentinel.Application.Common.Models;

namespace ElasticSentinel.Application.Features.Notifications;

/// <summary>
/// Handler for rendering notification templates using Scriban template engine.
/// Supports both single object and list-based templates.
/// </summary>
public interface IRenderNotificationHandler : IQueryHandler<RenderNotificationRequest, string>
{
}

/// <summary>
/// Handler for sending email notifications
/// </summary>
public interface ISendEmailNotificationHandler : ICommandHandler<SendEmailNotificationRequest>
{
}

/// <summary>
/// Handler for sending Microsoft Teams notifications
/// </summary>
public interface ISendTeamsNotificationHandler : ICommandHandler<SendTeamsNotificationRequest>
{
}

/// <summary>
/// Request to render a notification template with data
/// </summary>
public abstract record RenderNotificationRequest(string Template);

/// <summary>
/// Render notification with a single data object
/// </summary>
public record RenderNotificationWithDataRequest(string Template, Dictionary<string, string> Data)
    : RenderNotificationRequest(Template);

/// <summary>
/// Render notification with a list of data objects
/// </summary>
public record RenderNotificationWithListRequest(string Template, List<Dictionary<string, string>> DataList)
    : RenderNotificationRequest(Template);

/// <summary>
/// Request to send an email notification
/// </summary>
public record SendEmailNotificationRequest(string Message, EmailConnectorDetails EmailConnectorDetails);

/// <summary>
/// Request to send a Teams notification
/// </summary>
public record SendTeamsNotificationRequest(string Message, TeamsConnectorDetails TeamsConnectorDetails);
