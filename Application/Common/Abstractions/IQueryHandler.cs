namespace ElasticSentinel.Application.Common.Abstractions;

/// <summary>
/// Base interface for query handlers that perform read operations.
/// Queries don't modify state, they only return data.
/// </summary>
/// <typeparam name="TRequest">The query request type</typeparam>
/// <typeparam name="TResponse">The response type containing the queried data</typeparam>
public interface IQueryHandler<in TRequest, TResponse> : IHandler
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
