using ElasticSentinel.Application.Common.Abstractions;
using Scriban;

namespace ElasticSentinel.Application.Features.Notifications;

/// <summary>
/// Renders notification templates using the Scriban template engine.
/// Supports rendering with either a single data object or a list.
/// </summary>
internal sealed class RenderNotificationHandler : IRenderNotificationHandler
{
    public async Task<string> HandleAsync(
        RenderNotificationRequest request,
        CancellationToken cancellationToken = default)
    {
        var template = Template.Parse(request.Template);

        return request switch
        {
            RenderNotificationWithDataRequest dataRequest =>
                await template.RenderAsync(dataRequest.Data),

            RenderNotificationWithListRequest listRequest =>
                await template.RenderAsync(new { items = listRequest.DataList }),

            _ => throw new ArgumentException($"Unsupported notification request type: {request.GetType().Name}")
        };
    }
}
