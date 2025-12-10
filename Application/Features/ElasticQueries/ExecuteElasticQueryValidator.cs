using FluentValidation;

namespace ElasticSentinel.Application.Features.ElasticQueries;

public class ExecuteElasticQueryValidator : AbstractValidator<ExecuteElasticQueryRequest>
{
    public ExecuteElasticQueryValidator()
    {
        RuleFor(x => x.ApiRequest)
            .NotNull()
            .WithMessage("API Request cannot be null");

        RuleFor(x => x.ApiRequest.ElasticHost)
            .NotEmpty()
            .When(x => x.ApiRequest != null)
            .WithMessage("Elasticsearch host is required");

        RuleFor(x => x.ApiRequest.UserName)
            .NotEmpty()
            .When(x => x.ApiRequest != null)
            .WithMessage("Username is required");

        RuleFor(x => x.ApiRequest.Password)
            .NotEmpty()
            .When(x => x.ApiRequest != null)
            .WithMessage("Password is required");

        RuleFor(x => x.ApiRequest.QueryName)
            .NotEmpty()
            .When(x => x.ApiRequest != null)
            .WithMessage("Query name is required");

        RuleFor(x => x.ResponseMap)
            .NotNull()
            .NotEmpty()
            .WithMessage("Response map cannot be null or empty");

        RuleFor(x => x.Logger)
            .NotNull()
            .WithMessage("Logger cannot be null");
    }
}
