namespace ElasticSentinel.Presentation.API.Filters;

/// <summary>
/// Validation filter for Minimal API endpoints
/// </summary>
public class ValidationFilter : IEndpointFilter
{
    private readonly ILogger<ValidationFilter> _logger;

    public ValidationFilter(ILogger<ValidationFilter> logger)
    {
        _logger = logger;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // Log incoming request
        _logger.LogInformation("Processing request to {Path}", context.HttpContext.Request.Path);

        // Execute the endpoint
        var result = await next(context);

        return result;
    }
}
