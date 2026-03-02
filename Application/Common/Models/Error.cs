namespace ElasticSentinel.Application.Common.Models;

/// <summary>
/// Represents an error with a code and message
/// </summary>
public sealed record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");
    
    public static Error NotFound(string entityName, object id) =>
        new("Error.NotFound", $"{entityName} with id '{id}' was not found");
    
    public static Error Validation(string message) =>
        new("Error.Validation", message);
    
    public static Error Failure(string code, string message) =>
        new(code, message);
}
