using FluentValidation;

namespace ElasticSentinel.Application.Features.Notifications;

public class SendEmailNotificationValidator : AbstractValidator<SendEmailNotificationRequest>
{
    public SendEmailNotificationValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Message content is required");

        RuleFor(x => x.EmailConnectorDetails)
            .NotNull()
            .WithMessage("Email connector details are required");

        RuleFor(x => x.EmailConnectorDetails.FromEmail)
            .NotEmpty()
            .EmailAddress()
            .When(x => x.EmailConnectorDetails != null)
            .WithMessage("Valid 'From' email address is required");

        RuleFor(x => x.EmailConnectorDetails.ToEmails)
            .NotEmpty()
            .When(x => x.EmailConnectorDetails != null)
            .WithMessage("'To' email address is required");

        RuleFor(x => x.EmailConnectorDetails.SMTPServer)
            .NotEmpty()
            .When(x => x.EmailConnectorDetails != null)
            .WithMessage("SMTP server is required");

        RuleFor(x => x.EmailConnectorDetails.SMTPPort)
            .GreaterThan(0)
            .LessThanOrEqualTo(65535)
            .When(x => x.EmailConnectorDetails != null)
            .WithMessage("Valid port number is required (1-65535)");
    }
}
