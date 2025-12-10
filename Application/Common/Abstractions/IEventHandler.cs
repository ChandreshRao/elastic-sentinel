namespace ElasticSentinel.Application.Common.Abstractions;

/// <summary>
/// Marker interface for all event handlers in the application.
/// Used for automatic event handler registration in DI container.
/// </summary>
public interface IEventHandler
{
}

/// <summary>
/// Base interface for typed event handlers.
/// Event handlers react to events that have already occurred.
/// Multiple handlers can handle the same event.
/// </summary>
/// <typeparam name="TEvent">Type of event handled by this handler</typeparam>
public interface IEventHandler<in TEvent> : IEventHandler where TEvent : IEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}
