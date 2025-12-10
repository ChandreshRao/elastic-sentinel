using ElasticSentinel.Application.Common.Abstractions;

namespace ElasticSentinel.Application.Features.Alerts;

/// <summary>
/// Handler for rendering alert messages to HTML using Razor views.
/// This is a query handler as it renders views without modifying state.
/// </summary>
public interface IRenderAlertMessageHandler : IQueryHandler<RenderAlertMessageRequest, string>
{
}

/// <summary>
/// Request to render an alert message using a Razor view
/// </summary>
/// <param name="ViewName">The path to the Razor view</param>
/// <param name="Model">The model to pass to the view</param>
public record RenderAlertMessageRequest(string ViewName, object Model);
