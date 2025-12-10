using FluentValidation;

namespace ElasticSentinel.Application.Features.Notifications;

public class RenderNotificationValidator : AbstractValidator<RenderNotificationWithDataRequest>
{
    public RenderNotificationValidator()
    {
        RuleFor(x => x.Template)
            .NotEmpty()
            .WithMessage("Template content is required");

        RuleFor(x => x.Data)
            .NotNull()
            .WithMessage("Data cannot be null");
    }
}

public class RenderNotificationWithListValidator : AbstractValidator<RenderNotificationWithListRequest>
{
    public RenderNotificationWithListValidator()
    {
        RuleFor(x => x.Template)
            .NotEmpty()
            .WithMessage("Template content is required");

        RuleFor(x => x.DataList)
            .NotNull()
            .NotEmpty()
            .WithMessage("Data list cannot be null or empty");
    }
}
