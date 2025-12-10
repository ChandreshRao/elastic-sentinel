using FluentValidation;

namespace ElasticSentinel.Application.Features.Notifications;

public class SendTeamsNotificationValidator : AbstractValidator<SendTeamsNotificationRequest>
{
    public SendTeamsNotificationValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Message content is required");

        RuleFor(x => x.TeamsConnectorDetails)
            .NotNull()
            .WithMessage("Teams connector details are required");

        RuleFor(x => x.TeamsConnectorDetails.WebhookUrl)
            .NotEmpty()
            .Must(BeValidUrl)
            .When(x => x.TeamsConnectorDetails != null)
            .WithMessage("Valid webhook URL is required");
    }

    private bool BeValidUrl(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
