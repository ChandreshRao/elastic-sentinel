using ElasticSentinel.Application.Common.Abstractions;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ElasticSentinel.Application.Features.Notifications;

/// <summary>
/// Sends notifications to Microsoft Teams channels using incoming webhooks.
/// </summary>
internal sealed class SendTeamsNotificationHandler : ISendTeamsNotificationHandler
{
    public async Task HandleAsync(
        SendTeamsNotificationRequest request,
        CancellationToken cancellationToken = default)
    {
        var teamsConnectorDetails = request.TeamsConnectorDetails;

        using var httpClient = new HttpClient();
        using var httpRequest = new HttpRequestMessage(new HttpMethod("POST"), teamsConnectorDetails.WebhookUrl);

        var teamsMsg = new { text = request.Message.Replace("\r\n", " ") };
        httpRequest.Content = new StringContent(JsonConvert.SerializeObject(teamsMsg));
        httpRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

        var response = await httpClient.SendAsync(httpRequest, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
