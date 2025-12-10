namespace ElasticSentinel.Application.Common.Abstractions;

/// <summary>
/// Base interface for command handlers that perform write operations.
/// Commands modify state and don't return data (except success/failure).
/// </summary>
/// <typeparam name="TRequest">The command request type</typeparam>
/// <typeparam name="TResponse">The response type</typeparam>
public interface ICommandHandler<in TRequest, TResponse> : IHandler
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Command handler that doesn't return a value (void commands).
/// </summary>
/// <typeparam name="TRequest">The command request type</typeparam>
public interface ICommandHandler<in TRequest> : IHandler
{
    Task HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
