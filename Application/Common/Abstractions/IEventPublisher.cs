namespace ElasticSentinel.Application.Common.Abstractions;

/// <summary>
/// Interface for publishing domain events to registered event handlers.
/// Events are published to all registered handlers and executed in parallel.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes an event to all registered handlers.
    /// Handlers are executed in parallel using Task.WhenAll.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to publish</typeparam>
    /// <param name="event">The event instance</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent;
}
