using FluentValidation;

namespace ElasticSentinel.Application.Features.Alerts;

public class RenderAlertMessageValidator : AbstractValidator<RenderAlertMessageRequest>
{
    public RenderAlertMessageValidator()
    {
        RuleFor(x => x.ViewName)
            .NotEmpty()
            .WithMessage("View name is required");

        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Model cannot be null");
    }
}
